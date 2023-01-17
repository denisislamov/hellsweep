using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Pooling.Interfaces
{
	public interface ICompositeViewPool
	{
		void Add<T>(IViewPoolIdentity viewPoolIdentity, T prefab, int count = 32) where T : Component;
		bool TryGet<T>(IViewPoolIdentity viewPoolIdentity, out IViewPoolObject<T> result) where T : Component;
		bool TryRelease<T>(IViewPoolIdentity viewPoolIdentity, T obj) where T : Component;
	}
}