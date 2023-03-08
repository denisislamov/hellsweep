using TonPlay.Client.Roguelike.Core.Player.Views.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace TonPlay.Client.Roguelike.Core.Player.Views
{
	public class PlayerView : MonoBehaviour, IPlayerView
	{
		[SerializeField]
		private Rigidbody2D _rigidbody;

		[SerializeField]
		private Transform _weaponSpawnRoot;

		[SerializeField]
		private Animator _animator;
		
		[SerializeField]
		private Animator _bloodAnimator;
		
		[SerializeField]
		private float _attackAnimationDuration;

		[SerializeField]
		private SpriteRenderer[] _spriteRenderers;

		public Rigidbody2D Rigidbody2D => _rigidbody;
		
		public Animator Animator => _animator;
		
		public Animator BloodAnimator => _bloodAnimator;
		
		public SpriteRenderer[] SpriteRenderers => _spriteRenderers;
		
		public float AttackAnimationDuration => _attackAnimationDuration;

		public Vector2 Position => _rigidbody.position;

		public Transform WeaponSpawnRoot => _weaponSpawnRoot;
	}
}