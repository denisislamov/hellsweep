using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public abstract class CollectionPresenter<TCollectionItemView, TContext> 
		: Presenter<ICollectionView<TCollectionItemView>, TContext>,
		  ICollectionPresenter<TCollectionItemView, TContext>
		where TCollectionItemView : ICollectionItemView
		where TContext : IScreenContext
	{
		private readonly ICollectionItemPool<TCollectionItemView> _itemPool;
		private readonly List<TCollectionItemView> _items = new List<TCollectionItemView>();

		protected IReadOnlyCollection<TCollectionItemView> Items => _items;

		protected CollectionPresenter(
			ICollectionView<TCollectionItemView> view,
			TContext screenContext,
			ICollectionItemPool<TCollectionItemView> itemPool)
			: base(view, screenContext)
		{
			_itemPool = itemPool;
		}

		public override void Dispose()
		{
			base.Dispose();

			Clear();
		}

		protected TCollectionItemView Add()
		{
			var item = _itemPool.Spawn();
			View.AddContent(item);
			_items.Add(item);
			return item;
		}

		protected void Remove(TCollectionItemView item)
		{
			_items.Remove(item);
			_itemPool.Despawn(item);
		}

		protected void RemoveAt(int index)
		{
			var item = _items[index];
			_items.RemoveAt(index);
			_itemPool.Despawn(item);
		}

		protected void Clear()
		{
			foreach (var item in _items)
			{
				_itemPool.Despawn(item);
			}

			_items.Clear();
		}
	}
}