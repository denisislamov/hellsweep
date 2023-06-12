using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Pooling.Creators
{
	public abstract class PoolObjectCreator : ScriptableObject, IPoolObjectCreator
	{
		[SerializeField]
		private int _poolCount;

		[SerializeField]
		private PoolType _poolType;
		
		protected int PoolCount => _poolCount;
		
		protected PoolType PoolType => _poolType;
		
		protected abstract IViewPoolIdentity Identity { get; }

		public abstract void Create(ICompositeViewPool compositeViewPool);
	}
	
	public abstract class PoolObjectCreator<T>: PoolObjectCreator where T : Component
	{
		protected abstract T Prefab { get; }
		
		public override void Create(ICompositeViewPool compositeViewPool)
		{
			compositeViewPool.Add(Identity, Prefab, PoolCount, PoolType);
		}
	}
}