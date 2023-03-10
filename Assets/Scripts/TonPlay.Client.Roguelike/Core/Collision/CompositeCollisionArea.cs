using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Collision
{
	public class CompositeCollisionArea : CollisionArea, ICompositeCollisionArea
	{
		private List<ICollisionArea> _collisionAreas = new List<ICollisionArea>();

		public IReadOnlyList<ICollisionArea> CollisionAreas => _collisionAreas;

		public CompositeCollisionArea(ICompositeCollisionAreaConfig config) : base(config)
		{
			foreach (var configCollisionArea in config.CollisionAreaConfigs)
			{
				_collisionAreas.Add(CollisionAreaFactory.Create(configCollisionArea));
			}
		}
	}
}