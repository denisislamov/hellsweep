using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Pooling
{
	public class ViewPoolObject<T> : IViewPoolObject<T> where T : Component
	{
		private readonly IViewPool<T> _viewPool;
		private readonly T _obj;

		public T Object => _obj;

		public ViewPoolObject(IViewPool<T> viewPool, T obj)
		{
			_viewPool = viewPool;
			_obj = obj;
		}

		public void Release()
		{
			_viewPool.Release(_obj);
		}
	}
}