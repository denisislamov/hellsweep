using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Pooling.Interfaces
{
	public interface ICompositeViewPool
	{
		void Add<T>(IViewPoolIdentity viewPoolIdentity, T prefab, int count = 32, PoolType poolType = PoolType.FindInactive) where T : Component;
		bool TryGet<T>(IViewPoolIdentity viewPoolIdentity, out IViewPoolObject<T> result) where T : Component;
	}
}