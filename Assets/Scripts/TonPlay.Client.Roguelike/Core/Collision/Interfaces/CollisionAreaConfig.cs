using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	internal abstract class CollisionAreaConfig : ScriptableObject, ICollisionAreaConfig
	{
		[SerializeField]
		private bool _doNotInitiateCollisionOverlap;
		public bool DoNotInitiateCollisionOverlap => _doNotInitiateCollisionOverlap;
	}
}