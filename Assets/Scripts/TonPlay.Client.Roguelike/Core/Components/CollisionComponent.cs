using TonPlay.Client.Roguelike.Core.Collision.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct CollisionComponent
	{
		public ICollisionArea CollisionArea;
		public int LayerMask;
	}
}