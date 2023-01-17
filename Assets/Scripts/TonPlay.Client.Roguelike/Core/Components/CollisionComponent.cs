using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Components
{
	public struct CollisionComponent
	{
		public ICollisionAreaConfig CollisionAreaConfig;
		public int LayerMask;
	}
}