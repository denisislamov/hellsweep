using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Collision
{
	public static class CollisionAreaFactory
	{
		public static ICollisionArea Create(ICollisionAreaConfig config)
		{
			switch (config)
			{
				case ICompositeCollisionAreaConfig compositeCollisionAreaConfig:
				{
					return new CompositeCollisionArea(compositeCollisionAreaConfig);
				}
				default:
					return new CollisionArea(config);
			}
		}
	}
}