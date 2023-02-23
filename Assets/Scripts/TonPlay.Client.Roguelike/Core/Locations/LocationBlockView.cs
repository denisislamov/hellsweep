using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	public class LocationBlockView : MonoBehaviour, ILocationBlockView
	{
		[SerializeField]
		private BoxCollider2D _sizeBoxCollider;

		public Vector2 Size => _sizeBoxCollider.size;

		public void SetPosition(Vector2 position)
		{
			transform.position = position;
		}
	}
}