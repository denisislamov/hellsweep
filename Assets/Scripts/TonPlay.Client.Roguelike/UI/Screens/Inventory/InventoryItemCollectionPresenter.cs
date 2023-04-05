using TonPlay.Client.Common.Network.Interfaces;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemCollectionPresenter : CollectionPresenter<IInventoryItemView, IInventoryItemCollectionContext>
	{
		private readonly IInventoryItemPresentationProvider _itemPresentationProvider;
		private readonly InventoryItemPresenter.Factory _itemPresenterFactory;
		private readonly IInventoryItemsConfigProvider _itemsConfigProvider;
		private readonly IRestApiClient _restApiClient;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		public InventoryItemCollectionPresenter(
			ICollectionView<IInventoryItemView> view,
			IInventoryItemCollectionContext screenContext,
			ICollectionItemPool<IInventoryItemView> itemPool,
			IInventoryItemPresentationProvider itemPresentationProvider,
			InventoryItemPresenter.Factory itemPresenterFactory,
			IInventoryItemsConfigProvider itemsConfigProvider,
			IRestApiClient restApiClient,
			IMetaGameModelProvider metaGameModelProvider)
			: base(view, screenContext, itemPool)
		{
			_itemPresentationProvider = itemPresentationProvider;
			_itemPresenterFactory = itemPresenterFactory;
			_itemsConfigProvider = itemsConfigProvider;
			_restApiClient = restApiClient;
			_metaGameModelProvider = metaGameModelProvider;

			InitView();
		}

		private void InitView()
		{
			for (var i = 0; i < Context.Items.Count; i++)
			{
				var item = Context.Items[i];
				var itemId = item.DetailId.Value;
				var config = _itemsConfigProvider.Get(itemId);
				var icon = _itemPresentationProvider.GetIcon(itemId);
				var slotIcon = _itemPresentationProvider.GetSlotIcon(config.SlotName);

				_itemPresentationProvider.GetColors(config.Rarity, out var mainColor, out var backgroundGradient);

				var itemView = Add();
				var presenter = _itemPresenterFactory.Create(
					itemView,
					CreateItemContext(itemId, icon, slotIcon, mainColor, backgroundGradient, config, item));

				Presenters.Add(presenter);
			}
		}

		private IInventoryItemContext CreateItemContext(string itemId, Sprite icon, Sprite slotIcon, Color mainColor, Gradient backgroundGradient, IInventoryItemConfig config, IInventoryItemModel item)
			=> new InventoryItemContext(
				itemId,
				icon,
				slotIcon,
				mainColor,
				backgroundGradient,
				config.Name,
				item.Level.Value,
				() => ItemClickHandler(item));

		private async void ItemClickHandler(IInventoryItemModel item)
		{
			Debug.Log($"Has been clicked item with id {item.DetailId.Value}");

			var itemConfig = _itemsConfigProvider.Get(item.DetailId.Value);
			var requiredSlot = _metaGameModelProvider.Get().ProfileModel.InventoryModel.Slots[itemConfig.SlotName];

			var response = await _restApiClient.PutItem(
				new ItemPutBody()
				{
					itemDetailId = item.Id.Value,
					slotId = requiredSlot.Id.Value
				});

			if (response != null)
			{
				Clear();
				AddItemToSlotModel(item, requiredSlot);
				InitView();
			}
		}

		private static void AddItemToSlotModel(IInventoryItemModel item, ISlotModel requiredSlot)
		{
			var requiredSlotData = requiredSlot.ToData();
			requiredSlotData.Item = item.ToData();
			requiredSlot.Update(requiredSlotData);
		}

		public class Factory : PlaceholderFactory<IInventoryItemCollectionView, IInventoryItemCollectionContext, InventoryItemCollectionPresenter>
		{
		}
	}
}