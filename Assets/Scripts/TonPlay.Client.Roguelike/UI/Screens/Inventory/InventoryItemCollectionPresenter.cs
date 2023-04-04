using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemCollectionPresenter : CollectionPresenter<IInventoryItemView, IInventoryItemCollectionContext>
	{
		private readonly IInventoryItemPresentationProvider _itemPresentationProvider;
		private readonly InventoryItemPresenter.Factory _itemPresenterFactory;
		private readonly IInventoryItemsConfigProvider _itemsConfigProvider;

		public InventoryItemCollectionPresenter(
			ICollectionView<IInventoryItemView> view,
			IInventoryItemCollectionContext screenContext,
			ICollectionItemPool<IInventoryItemView> itemPool,
			IInventoryItemPresentationProvider itemPresentationProvider,
			InventoryItemPresenter.Factory itemPresenterFactory,
			IInventoryItemsConfigProvider itemsConfigProvider)
			: base(view, screenContext, itemPool)
		{
			_itemPresentationProvider = itemPresentationProvider;
			_itemPresenterFactory = itemPresenterFactory;
			_itemsConfigProvider = itemsConfigProvider;

			InitView();
		}

		private void InitView()
		{
			for (var i = 0; i < Context.Items.Count; i++)
			{
				var itemId = Context.Items[i];
				var config = _itemsConfigProvider.Get(itemId);
				var icon = _itemPresentationProvider.GetIcon(itemId);
				
				_itemPresentationProvider.GetColors(config.Rarity, out var mainColor, out var backgroundGradient);

				var itemView = Add();
				var presenter = _itemPresenterFactory.Create(
					itemView,
					new InventoryItemContext(itemId, icon, mainColor, backgroundGradient, config.Name));

				Presenters.Add(presenter);
			}
		}

		public class Factory : PlaceholderFactory<IInventoryItemCollectionView, IInventoryItemCollectionContext, InventoryItemCollectionPresenter>
		{
		}
	}
}