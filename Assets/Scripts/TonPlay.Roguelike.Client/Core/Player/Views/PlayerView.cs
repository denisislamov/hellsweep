using TonPlay.Roguelike.Client.Core.Player.Views.Intefacves;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Player.Views
{
	public class PlayerView : MonoBehaviour, IPlayerView
	{
		[SerializeField]
		private Rigidbody2D _rigidbody;

		public Rigidbody2D Rigidbody2D => _rigidbody;
		
		public Vector2 Position => _rigidbody.position;
	}
}