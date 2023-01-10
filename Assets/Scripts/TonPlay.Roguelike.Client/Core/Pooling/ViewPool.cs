using System;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Roguelike.Client.Core.Pooling
{
	public class ViewPool<T> : IViewPool<T> where T : Component
	{
		private readonly Vector3 _spawnPosition = Vector3.one * -100000f;
		private readonly IViewPoolObject<T>[] _pool;
		
		private int _pointer = 0;
		
		public ViewPool(T prefab, int size = 32)
		{
			if (size == 0)
			{
				throw new NotSupportedException();
			}
			
			_pool = new IViewPoolObject<T>[size];

			for (int i = 0; i < size; i++)
			{
				var obj = Instantiate(prefab);
				var transform = obj.transform;
				transform.position = _spawnPosition;
				obj.gameObject.SetActive(false);

				_pool[i] = new ViewPoolObject<T>(this, obj);
			}
		}
		
		private T Instantiate(T prefab)
		{
			var obj = Object.Instantiate(prefab);
			return obj;
		}

		public IViewPoolObject<T> Get()
		{
			var iterations = 0;
			while (iterations < _pool.Length)
			{
				iterations++;
				_pointer++;
				_pointer %= _pool.Length;

				if (!_pool[_pointer].Object.gameObject.activeSelf)
				{
					break;
				}
			}

			var obj = _pool[_pointer];
			obj.Object.gameObject.SetActive(true);
			
			return obj;
		}
		
		public void Release(T obj)
		{
			obj.transform.position = _spawnPosition;
			obj.gameObject.SetActive(false);
		}
	}
}