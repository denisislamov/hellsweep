using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Network;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing;
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
			IShopPurchaseActionContext context)
		{
			_uiService = uiService;
			_metaGameModelProvider = metaGameModelProvider;
			_context = context;
			_restApiClient = restApiClient;

			_reasonFuncs = new Dictionary<PaymentReason, Func<object, UniTask<Response<PaymentTransactionResponse>>>>()
			{
				[PaymentReason.ITEM] = (value) => restApiClient.PostBuyMarketItem(new BuyMarketItemPostBody(){ itemDetailId = (string) value }),
				[PaymentReason.KEYS] = (value) => restApiClient.PostBuyMarketKeys((RarityName)value),
				[PaymentReason.PACK] = (value) => restApiClient.PostBuyMarketPack(new BuyMarketPackPostBody(){ packId = (string) value }),
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
			_lockerScreen = _uiService.Open<ShopTransactionProcessingScreen, IScreenContext>(ScreenContext.Empty);

			var response = await _reasonFuncs[_context.PaymentReason].Invoke(_context.Value);
			if (!response.successful)
			{
				OnRequestFailed(response);
				return;
			}

			_currentPaymentTransactionResponse.SetValueAndForceNotify(response.response);

			_loopedTransactionRequestListener = Observable
											   .Timer(TimeSpan.FromSeconds(TIMER))
											   .Repeat()
											   .Subscribe((unit) => UpdateCurrentTransaction());
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
			var result = (PaymentStatus) Enum.Parse(typeof(PaymentStatus), responseStatus, true);
			return result;
		}

		private void ProcessBuyingBlueprintsTransaction(PaymentTransactionResponse response)
		{
			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var inventoryData = inventoryModel.ToData();

			inventoryData.SetBlueprintsValue(response.attributes.blueprintsSlotPurpose, response.attributes.amount);

			inventoryModel.Update(inventoryData);
		}

		private void ProcessBuyingEnergyTransaction(PaymentTransactionResponse response)
		{
			var balanceModel = _metaGameModelProvider.Get().ProfileModel.BalanceModel;
			var balanceData = balanceModel.ToData();

			balanceData.Energy = response.attributes.amount;

			balanceModel.Update(balanceData);
		}

		private void ProcessBuyingPackTransaction(PaymentTransactionResponse response)
		{

		}

		private void ProcessBuyingKeysTransaction(PaymentTransactionResponse response)
		{
			var inventoryModel = _metaGameModelProvider.Get().ProfileModel.InventoryModel;
			var inventoryData = inventoryModel.ToData();

			inventoryData.SetKeysValue(response.attributes.rarity, Convert.ToInt32(response.attributes.amount));

			inventoryModel.Update(inventoryData);
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
		}

		public class Factory : PlaceholderFactory<IShopPurchaseActionContext, ShopPurchaseAction>
		{
		}
	}
}