using TonPlay.Roguelike.Client.Core.Enemies.Views.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Enemies.Views
{
	public abstract class EnemyView : MonoBehaviour, IEnemyView
	{
		[SerializeField]
		private Rigidbody2D _rigidbody;
		
		[SerializeField]
		private Collider2D _collider;

		public Collider2D Collider2D => _collider;
		
		public Rigidbody2D Rigidbody2D => _rigidbody;
	}
}