using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Pooling
{
	public class ViewPoolObject<T> : IViewPoolObject<T> where T : Component
	{
		private readonly IViewPool<T> _viewPool;
		private readonly T _obj;
		private bool _active;

		public T Object => _obj;
		public bool Active => _active;

		public ViewPoolObject(IViewPool<T> viewPool, T obj)
		{
			_viewPool = viewPool;
			_obj = obj;
		}

		public void Release()
		{
			_viewPool.Release(this);
		}

		public void SetActive(bool state)
		{
			_active = state;
		}
	}
}