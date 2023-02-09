using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces
{
	public interface IAABBCollisionAreaConfig : ICollisionAreaConfig
	{
		Rect Rect { get; }
	}
}