using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Pooling.Interfaces
{
	public interface IViewPool<T> where T : Component
	{
		IViewPoolObject<T> Get();

		void Release(T obj);

		void IncreaseSize(int count);
	}
}