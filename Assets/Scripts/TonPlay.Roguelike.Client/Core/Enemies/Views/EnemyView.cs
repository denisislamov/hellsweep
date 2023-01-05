using TonPlay.Roguelike.Client.Core.Enemies.Views.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Enemies.Views
{
	public abstract class EnemyView : MonoBehaviour, IEnemyView
	{
		[SerializeField]
		private Rigidbody2D _rigidbody;

		public Rigidbody2D Rigidbody2D => _rigidbody;
	}
}