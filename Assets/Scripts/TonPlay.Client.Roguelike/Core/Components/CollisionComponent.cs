using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct CollisionComponent
	{
		public ICollisionAreaConfig CollisionAreaConfig;
		public int LayerMask;
	}
}