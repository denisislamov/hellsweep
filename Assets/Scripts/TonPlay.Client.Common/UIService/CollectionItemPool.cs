using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.UIService.Utilities
{
	public class CollectionItemPool<TItemContract, TItem> : MonoMemoryPool<TItem>, ICollectionItemPool<TItemContract>
		where TItemContract : ICollectionItemView
		where TItem : Component, TItemContract
	{
		public new TItemContract Spawn() => base.Spawn();

		public void Despawn(TItemContract itemView) => base.Despawn((TItem) itemView);
	}
}