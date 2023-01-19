using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Collectables.Config
{
	public abstract class CollectableConfig : ScriptableObject, ICollectableConfig
	{
		[SerializeField]
		private string _id;
		
		[SerializeField]
		private float _value;
		
		[SerializeField]
		private CollectableView _prefab;
		
		[SerializeField]
		private CollisionAreaConfig _collisionAreaConfig;

		[SerializeField]
		private int _poolSize = 512;

		[SerializeField, Layer]
		private int _layer;

		public abstract CollectableType Type { get; }
		public string Id => _id;
		public float Value => _value;
		public CollectableView Prefab => _prefab;
		public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
		public int Layer => _layer;
		public int PoolSize => _poolSize;
	}
}