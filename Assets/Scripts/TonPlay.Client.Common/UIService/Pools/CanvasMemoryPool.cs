using UnityEngine;
using Zenject;

namespace TonPlay.Client.Common.UIService.Pools
{
	public class CanvasMemoryPool<TValue> : MemoryPool<TValue>
		where TValue : ICanvasCollectionItemView
	{
		protected override void OnCreated(TValue item)
		{
			item.Canvas.enabled = false;
		}

		protected override void OnDestroyed(TValue item)
		{
			item.Canvas.enabled = false;
		}

		protected override void OnSpawned(TValue item)
		{
			item.Canvas.enabled = true;
		}

		protected override void OnDespawned(TValue item)
		{
			item.Canvas.enabled = false;
		}
	}
}