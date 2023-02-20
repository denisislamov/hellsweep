using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Enemies.Views.Interfaces;
using TonPlay.Roguelike.Client.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Views
{
	public abstract class EnemyView : MonoBehaviour, IEnemyView, IEntityIdProvider
	{
		[SerializeField]
		private Rigidbody2D _rigidbody;
		
		[SerializeField]
		private Collider2D _collider;
		
		private int? _entityId;

		public Collider2D Collider2D => _collider;
		
		public Rigidbody2D Rigidbody2D => _rigidbody;

		public int EntityId => _entityId ?? EcsEntity.DEFAULT_ID;

		public void SetEntityId(int id)
		{
			_entityId = id;
		}
	}
}