using System;
using System.Collections.Generic;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Pooling
{
	public class CompositeViewPool : ICompositeViewPool
	{
		private readonly Dictionary<string, object> _pools = new Dictionary<string, object>(8);
		
		public void Add<T>(IViewPoolIdentity viewPoolIdentity, T prefab, int count = 32) where T : Component
		{
			if (_pools.ContainsKey(viewPoolIdentity.Id))
			{
				throw new NotSupportedException();
			}
			
			_pools.Add(viewPoolIdentity.Id, new ViewPool<T>(prefab, count));
		}
		
		public bool TryGet<T>(IViewPoolIdentity viewPoolIdentity, out IViewPoolObject<T> result) where T : Component
		{
			if (!_pools.ContainsKey(viewPoolIdentity.Id))
			{
				result = default(IViewPoolObject<T>);
				return false;
			}

			result = ((IViewPool<T>)_pools[viewPoolIdentity.Id]).Get();
			return true;
		}
		
		public bool TryRelease<T>(IViewPoolIdentity viewPoolIdentity, T obj) where T : Component
		{
			if (!_pools.ContainsKey(viewPoolIdentity.Id))
			{
				return false;
			}

			((IViewPool<T>)_pools[viewPoolIdentity.Id]).Release(obj);
			return true;
		}
	}
}