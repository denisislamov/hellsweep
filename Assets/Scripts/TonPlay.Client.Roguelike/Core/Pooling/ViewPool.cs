using System;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Pooling
{
	public class ViewPool<T> : IViewPool<T> where T : Component
	{
		private readonly T _prefab;
		private readonly PoolType _poolType;
		private readonly Vector3 _spawnPosition = Vector3.one*-100000f;

		private IViewPoolObject<T>[] _pool;

		private int _pointer = 0;

		public ViewPool(T prefab, int size = 32, PoolType poolType = PoolType.FindInactive)
		{
			_prefab = prefab;
			_poolType = poolType;

			if (size == 0)
			{
				throw new NotSupportedException();
			}

			_pool = new IViewPoolObject<T>[size];

			for (int i = 0; i < size; i++)
			{
				var obj = CreateObject(prefab);

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
			switch (_poolType)
			{
				case PoolType.Loop:
				{
					SetCurrentPointerWithLoopMethod();
					break;
				}
				case PoolType.FindInactive:
				default:
				{
					SetCurrentPointerWithFindInactiveMethod();
					break;
				}
			}
			
			_pointer++;
			_pointer %= _pool.Length;

			var obj = _pool[_pointer];
			
			obj.SetActive(true);

			return obj;
		}
		
		private void SetCurrentPointerWithFindInactiveMethod()
		{
			var iterations = 0;
			while (iterations < _pool.Length)
			{
				iterations++;
				_pointer++;
				_pointer %= _pool.Length;

				if (!_pool[_pointer].Active)
				{
					break;
				}
			}
		}
		
		private void SetCurrentPointerWithLoopMethod()
        {
        	_pointer++;
        	_pointer %= _pool.Length;
        }

		public void Release(ViewPoolObject<T> viewPoolObject)
		{
			viewPoolObject.Object.transform.position = _spawnPosition;
			viewPoolObject.SetActive(false);
			viewPoolObject.EntityId = EcsEntity.DEFAULT_ID;
		}

		public void IncreaseSize(int count)
		{
			var currentSize = _pool.Length;

			Array.Resize(ref _pool, currentSize + count);

			for (int i = 0; i < count; i++)
			{
				var obj = CreateObject(_prefab);

				_pool[i + currentSize] = new ViewPoolObject<T>(this, obj);
			}
		}

		private T CreateObject(T prefab)
		{
			var obj = Instantiate(prefab);
			var transform = obj.transform;
			transform.position = _spawnPosition;
			obj.gameObject.SetActive(true);

			return obj;
		}
	}
}