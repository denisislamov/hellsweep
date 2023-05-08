using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Pools;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;

namespace TonPlay.Client.Common.UIService
{
	public class CanvasCollectionItemPool<TItemContract, TItem> : CanvasMemoryPool<TItem>, ICollectionItemPool<TItemContract>
		where TItemContract : ICollectionItemView
		where TItem : TItemContract, ICanvasCollectionItemView
	{
		public new TItemContract Spawn() => base.Spawn();

		public void Despawn(TItemContract itemView) => base.Despawn((TItem) itemView);
	}
}