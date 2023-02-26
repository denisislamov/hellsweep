using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces
{
	public interface ICollisionAreaConfig
	{
		Vector2 Position { get; }
		
		bool DoNotInitiateCollisionOverlap { get; }
	}
}