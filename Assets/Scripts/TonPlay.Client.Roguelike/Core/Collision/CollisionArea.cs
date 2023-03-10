using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision
{
	public class CollisionArea : ICollisionArea
	{
		private readonly Vector2 _position;

		public Vector2 Position => _position + Offset;
		
		public bool DoNotInitiateCollisionOverlap { get; }

		public ICollisionAreaConfig Config { get; }
		
		public float Scale { get; set; }
		
		public Vector2 Offset { get; set; }

		public CollisionArea(ICollisionAreaConfig config)
		{
			Config = config;
			_position = config.Position;
			DoNotInitiateCollisionOverlap = config.DoNotInitiateCollisionOverlap;
			Scale = 1f;
		}
	}
}