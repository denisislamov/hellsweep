using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ExplosionComponent
	{
		public ICollisionAreaConfig CollisionAreaConfig;
		public float Damage;
		public int LayerMask;
	}
}