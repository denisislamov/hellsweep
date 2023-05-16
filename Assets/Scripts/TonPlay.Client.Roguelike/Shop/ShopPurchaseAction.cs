using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Network;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Screens.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Shop
{
	public class ShopPurchaseAction
	{
		private const float TIMER = 7.5f;

		private readonly IUIService _uiService;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IShopPurchaseActionContext _context;
		private readonly IShopRewardPresentationProvider _shopRewardPresentationProvider;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IRestApiClient _restApiClient;

		private readonly IReadOnlyDictionary<PaymentReason, Func<object, UniTask<Response<PaymentTransactionResponse>>>> _reasonFuncs;
		private readonly IReadOnlyDictionary<PaymentReason, Action<PaymentTransactionResponse>> _responseActions;

		private readonly ReactiveProperty<PaymentTransactionResponse> _currentPaymentTransactionResponse = new ReactiveProperty<PaymentTransactionResponse>();

		private readonly List<IShopRewardItemContext> _rewardsResults = new List<IShopRewardItemContext>();

		private IDisposable _currentPaymentStatusListener;
		private IDisposable _loopedTransactionRequestListener;
		private IScreen _lockerScreen;
		public UniTaskCompletionSource<IShopPurchaseActionResult> CompletionSource { get; } = new UniTaskCompletionSource<IShopPurchaseActionResult>();

		public ShopPurchaseAction(
			IRestApiClient restApiClient,
			IUIService uiService,
			IMetaGameModelProvider metaGameModelProvider,
			IShopPurchaseActionContext context,
			IShopRewardPresentationProvider shopRewardPresentationProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider)
		{
			_uiService = uiService;
			_metaGameModelProvider = metaGameModelProvider;
			_context = context;
			_shopRewardPresentationProvider = shopRewardPresentationProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_restApiClient = restApiClient;

			_reasonFuncs = new Dictionary<PaymentReason, Func<object, UniTask<Response<PaymentTransactionResponse>>>>()
			{
				[PaymentReason.ITEM] = (value) => restApiClient.PostBuyMarketItem(new BuyMarketItemPostBody() {itemDetailId = (string)value}),
				[PaymentReason.KEYS] = (value) => restApiClient.PostBuyMarketKeys((RarityName)value),
				[PaymentReason.PACK] = (value) => restApiClient.PostBuyMarketPack(new BuyMarketPackPostBody() {packId = (string)value}),
				[PaymentReason.ENERGY] = (value) => restApiClient.PostBuyMarketEnergy(),
				[PaymentReason.BLUEPRINTS] = (value) => restApiClient.PostBuyMarketBlueprints(),
			};

			_responseActions = new Dictionary<PaymentReason, Action<PaymentTransactionResponse>>()
			{
				[PaymentReason.ITEM] = ProcessBuyingItemTransaction,
				[PaymentReason.KEYS] = ProcessBuyingKeysTransaction,
				[PaymentReason.PACK] = ProcessBuyingPackTransaction,
				[PaymentReason.ENERGY] = ProcessBuyingEnergyTransaction,
				[PaymentReason.BLUEPRINTS] = ProcessBuyingBlueprintsTransaction,
			};

			AddCurrentPaymentStatusSubscription();
		}

		public async UniTask Begin()
		{
			var tonkeeperReactiveProperty = new ReactiveProperty<string>(null);
			var responseReceivedReactiveProperty = new ReactiveProperty<bool>(false);

			_lockerScreen = _uiService.Open<ShopTransactionProcessingScreen, IShopTransactionProcessingScreenContext>(
				new ShopTransactionProcessingScreenContext(
					tonkeeperReactiveProperty,
					responseReceivedReactiveProperty,
					LockerCloseButtonClickCallback));

			var response = await _reasonFuncs[_context.PaymentReason].Invoke(_context.Value);

			responseReceivedReactiveProperty.SetValueAndForceNotify(true);

			if (!response.successful)
			{
				OnRequestFailed(response);
				return;
			}

			tonkeeperReactiveProperty.SetValueAndForceNotify(response.response.tonPayInResponse.tonkeeper);

			_currentPaymentTransactionResponse.SetValueAndForceNotify(response.response);

			_loopedTransactionRequestListener = Observable
											   .Timer(TimeSpan.FromSeconds(TIMER))
											   .Repeat()
											   .Subscribe((unit) => UpdateCurrentTransaction());
		}

		private void LockerCloseButtonClickCallback()
		{
			OnRequestFailed(null);
		}

		private void OnRequestFailed(Response<PaymentTransactionResponse> response)
		{
			_currentPaymentStatusListener?.Dispose();
			_loopedTransactionRequestListener?.Dispose();
			_uiService.Close(_lockerScreen);
			CompletionSource.TrySetResult(new ShopPurchaseActionResult(PaymentStatus.FAILED, _rewardsResults));
		}

		private async void UpdateCurrentTransaction()
		{
			if (_context.CancellationToken.IsCancellationRequested)
			{
				_currentPaymentStatusListener?.Dispose();
				_loopedTransactionRequestListener?.Dispose();
				CompletionSource.TrySetResult(new ShopPurchaseActionResult(PaymentStatus.FAILED, _rewardsResults));
				return;
			}

			var response = await _restApiClient.GetPaymentTransaction(_currentPaymentTransactionResponse.Value.id);
			if (!response.successful)
			{
				OnRequestFailed(response);
				return;
			}

			_currentPaymentTransactionResponse.SetValueAndForceNotify(response.response);
		}

		private void AddCurrentPaymentStatusSubscription()
		{
			_currentPaymentStatusListener = _currentPaymentTransactionResponse.Subscribe(CurrentPaymentTransactionHandler);
		}

		private void CurrentPaymentTransactionHandler(PaymentTransactionResponse response)
		{
			if (response == null)
			{
				return;
			}

			var debugJson = JsonUtility.ToJson(response);

			if (!string.IsNullOrEmpty(debugJson))
			{
				Debug.Log(debugJson);
			}

			if (ConvertStatusToEnum(response.status) == PaymentStatus.IN_PROCESS)
			{
				return;
			}

			_currentPaymentStatusListener?.Dispose();

			if (ConvertStatusToEnum(response.status) == PaymentStatus.COMPLETED)
			{
				_responseActions[_context.PaymentReason].Invoke(response);
			}

			_uiService.Close(_lockerScreen);

			CompletionSource.TrySetResult(new ShopPurchaseActionResult(PaymentStatus.COMPLETED, _rewardsResults));
		}

		private PaymentStatus ConvertStatusToEnum(string responseStatus)
		{
			var result = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), responseStatus, true);
			return result;
		}

		private IShopRewardItemContext CreateRewardContext(string rewardId, ulong amount)
		{
			var presentation = _shopRewardPresentationProvider.GetRewardPresentation(rewardId);

			if (presentation == null)
			{
				var config = _inventoryItemsConfigProvider.GetConfigByDetailId(rewardId);

				if (config == null)
				{
					Debug.LogWarning($"[ShopPurchaseAction] Presentation for reward {rewardId} hasn't been found");
					return null;
				}

				var itemPresentation = _inventoryItemPresentationProvider.GetItemPresentation(config.Id);

				if (itemPresentation == null)
				{
					Debug.LogWarning($"[ShopPurchaseAction] Presentation for reward {rewardId} hasn't been found");
					return null;
				}


				_inventoryItemPresentationProvider.GetColors(config.Rarity, out var color, out var gradient);

				return new ShopRewardItemContext(itemPresentation.Title, itemPresentation.Icon, gradient);
			}

			return new ShopRewardItemContext($"x{amount.ConvertToSuffixedFormat(1000, 2)}", presentation.Icon, presentation.BackgroundGradientMaterial);
		}

		private void ProcessBuyingBlueprintsTransaction(PaymentTransactionResponse response)
		{
			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var inventoryData = inventoryModel.ToData();

			inventoryData.SetBlueprintsValue(
				response.attributes.blueprintsSlotPurpose,
				inventoryData.GetBlueprintsValue(response.attributes.blueprintsSlotPurpose) + response.attributes.amount);

			inventoryModel.Update(inventoryData);

			_rewardsResults.Add(
				CreateRewardContext(
					$"blueprints_{response.attributes.blueprintsSlotPurpose.ToString().ToLowerInvariant()}",
					Convert.ToUInt64(inventoryData.GetBlueprintsValue(response.attributes.blueprintsSlotPurpose))));
		}

		private void ProcessBuyingEnergyTransaction(PaymentTransactionResponse response)
		{
			var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
			var balanceData = balanceModel.ToData();

			balanceData.Energy += response.attributes.amount;

			balanceModel.Update(balanceData);

			_rewardsResults.Add(CreateRewardContext("energy", Convert.ToUInt64(balanceModel.Energy.Value)));
		}

		private void ProcessBuyingKeysTransaction(PaymentTransactionResponse response)
		{
			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var inventoryData = inventoryModel.ToData();

			inventoryData.SetKeysValue(
				response.attributes.rarity,
				inventoryData.GetKeysValue(response.attributes.rarity) + Convert.ToInt32(response.attributes.amount));

			inventoryModel.Update(inventoryData);

			_rewardsResults.Add(
				CreateRewardContext(
					$"keys_{response.attributes.rarity.ToString().ToLowerInvariant()}",
					Convert.ToUInt64(inventoryData.GetKeysValue(response.attributes.rarity))));
		}

		private void ProcessBuyingItemTransaction(PaymentTransactionResponse response)
		{
			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var inventoryData = inventoryModel.ToData();

			var itemData = new InventoryItemData()
			{
				Id = response.attributes.id,
				ItemId = response.attributes.itemId,
				DetailId = response.attributes.itemDetailId,
			};

			inventoryData.Items.Add(itemData);

			inventoryModel.Update(inventoryData);

			_rewardsResults.Add(
				CreateRewardContext(response.attributes.itemDetailId, 1));
		}

		private void ProcessBuyingPackTransaction(PaymentTransactionResponse response)
		{
			var model = _metaGameModelProvider.Get().ShopModel.Packs.FirstOrDefault(_ => _.Id == response.packRateId);

			if (model == null)
			{
				Debug.LogError($"Shop Pack Model with id {response.packRateId} hasn't been found!");
				return;
			}

			if (model.Rewards.Coins > 0)
			{
				_rewardsResults.Add(CreateRewardContext("coins", model.Rewards.Coins));
				var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
				var balanceData = balanceModel.ToData();
				balanceData.Gold += Convert.ToInt64(model.Rewards.Coins);
				balanceModel.Update(balanceData);
			}

			if (model.Rewards.Energy > 0)
			{
				_rewardsResults.Add(CreateRewardContext("energy", model.Rewards.Energy));
				var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
				var balanceData = balanceModel.ToData();
				balanceData.Energy += Convert.ToInt64(model.Rewards.Energy);
				balanceModel.Update(balanceData);
			}

			if (model.Rewards.Blueprints > 0)
			{
				_rewardsResults.Add(
					CreateRewardContext(
						$"blueprints_{response.attributes.blueprintsSlotPurpose.ToString().ToLowerInvariant()}",
						model.Rewards.Blueprints));

				var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
				var inventoryData = inventoryModel.ToData();
				inventoryData.SetBlueprintsValue(response.attributes.blueprintsSlotPurpose, Convert.ToInt64(model.Rewards.Blueprints));
				inventoryModel.Update(inventoryData);
			}

			//todo: uncomment it if we would use heroSkins
			// if (model.Rewards.HeroSkins > 0)
			// {
			// 	_rewardsResults.Add(CreateRewardContext("hero_skins", Context.RewardsModel.HeroSkins));
			// }

			if (model.Rewards.KeysCommon > 0)
			{
				_rewardsResults.Add(CreateRewardContext("keys_common", model.Rewards.KeysCommon));

				var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
				var inventoryData = inventoryModel.ToData();
				inventoryData.SetKeysValue(RarityName.COMMON,
					Convert.ToInt32(inventoryData.GetKeysValue(RarityName.COMMON) + model.Rewards.KeysCommon));
				inventoryModel.Update(inventoryData);
			}

			if (model.Rewards.KeysUncommon > 0)
			{
				_rewardsResults.Add(CreateRewardContext("keys_uncommon", model.Rewards.KeysUncommon));

				var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
				var inventoryData = inventoryModel.ToData();
				inventoryData.SetKeysValue(RarityName.UNCOMMON,
					Convert.ToInt32(inventoryData.GetKeysValue(RarityName.UNCOMMON) + model.Rewards.KeysUncommon));
				inventoryModel.Update(inventoryData);
			}

			if (model.Rewards.KeysRare > 0)
			{
				_rewardsResults.Add(CreateRewardContext("keys_rare", model.Rewards.KeysRare));

				var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
				var inventoryData = inventoryModel.ToData();
				inventoryData.SetKeysValue(RarityName.RARE,
					Convert.ToInt32(inventoryData.GetKeysValue(RarityName.RARE) + model.Rewards.KeysRare));
				inventoryModel.Update(inventoryData);
			}

			if (model.Rewards.KeysLegendary > 0)
			{
				_rewardsResults.Add(CreateRewardContext("keys_legendary", model.Rewards.KeysLegendary));

				var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
				var inventoryData = inventoryModel.ToData();
				inventoryData.SetKeysValue(RarityName.LEGENDARY,
					Convert.ToInt32(inventoryData.GetKeysValue(RarityName.LEGENDARY) + model.Rewards.KeysLegendary));
				inventoryModel.Update(inventoryData);
			}
		}

		public class Factory : PlaceholderFactory<IShopPurchaseActionContext, ShopPurchaseAction>
		{
		}
	}
}