using TonPlay.Client.Common.Network.Interfaces;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using UniRx;
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
		private readonly DictionaryExt<string, InventoryItemContext> _itemContexts = new DictionaryExt<string, InventoryItemContext>();

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
				var item = Context.Items[i].Model;
				var itemId = item.DetailId.Value;
				var config = _itemsConfigProvider.Get(itemId);
				var presentation = _itemPresentationProvider.GetItemPresentation(itemId);
				var icon = presentation?.Icon ? presentation.Icon : _itemPresentationProvider.DefaultItemIcon;
				var slotIcon = _itemPresentationProvider.GetSlotIcon(config.SlotName);
				var itemEquippedState = Context.Items[i].EquippedState;

				_itemPresentationProvider.GetColors(config.Rarity, out var mainColor, out var backgroundGradientMaterial);

				var itemView = Add();
				var context = CreateItemContext(itemId, icon, slotIcon, mainColor, backgroundGradientMaterial, itemEquippedState, config, item);
				var presenter = _itemPresenterFactory.Create(
					itemView,
					context);
				
				_itemContexts.Add(item.Id.Value, context);
				
				Presenters.Add(presenter);
			}
		}

		private InventoryItemContext CreateItemContext(
			string itemId, 
			Sprite icon,
			Sprite slotIcon, 
			Color mainColor, 
			Material backgroundGradientMaterial,
			IReadOnlyReactiveProperty<bool> isEquipped,
			IInventoryItemConfig config, 
			IInventoryItemModel item)
			=> new InventoryItemContext(
				itemId,
				icon,
				slotIcon,
				mainColor,
				backgroundGradientMaterial,
				config.Name,
				item.Level.Value,
				isEquipped: isEquipped,
				() => Context.ItemClickCallback?.Invoke(item));

		public class Factory : PlaceholderFactory<IInventoryItemCollectionView, IInventoryItemCollectionContext, InventoryItemCollectionPresenter>
		{
		}
	}
}