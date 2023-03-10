using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ExplosionComponent
	{
		public ICollisionArea CollisionArea;
		public IDamageProvider DamageProvider;
		public int LayerMask;
	}
}