using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Enemies.Views.Interfaces;
using TonPlay.Roguelike.Client.Core.Interfaces;
using UnityEngine;
using UnityEngine.Playables;

namespace TonPlay.Client.Roguelike.Core.Enemies.Views
{
	public abstract class EnemyView : MonoBehaviour, IEnemyView, IEntityIdProvider
	{
		[SerializeField]
		private Rigidbody2D _rigidbody;

		[SerializeField]
		private Collider2D _collider;

		[SerializeField]
		private Animator _animator;
		
		[SerializeField]
		private SpriteRenderer[] _spriteRenderers;

		[SerializeField]
		private PlayableDirector _playableDirector;

		public Collider2D Collider2D => _collider;

		public Rigidbody2D Rigidbody2D => _rigidbody;
		public Animator Animator => _animator;
		public SpriteRenderer[] SpriteRenderers => _spriteRenderers;

		public PlayableDirector PlayableDirector => _playableDirector;

		private int? _entityId;

		public int EntityId => _entityId ?? EcsEntity.DEFAULT_ID;

		public void SetEntityId(int id)
		{
			_entityId = id;
		}
	}
}