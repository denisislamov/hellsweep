using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	public interface ICollisionArea
	{
		ICollisionAreaConfig Config { get; } 
		
		float Scale { get; set; }
		
		Vector2 Offset { get; set; }
		
		Vector2 Position { get; }
	}
}