using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Pooling.Interfaces
{
	public interface IViewPool<T> where T : Component
	{
		IViewPoolObject<T> Get();

		void Release(ViewPoolObject<T> viewPoolObject);

		void IncreaseSize(int count);
	}
}