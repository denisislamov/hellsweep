using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Pooling.Interfaces
{
	public interface IViewPool<T> where T : Component
	{
		IViewPoolObject<T> Get();

		void Release(T obj);
	}
}