using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ExplodeOnCollisionComponent
	{
		public IDamageProvider DamageProvider;
		public ICollisionAreaConfig CollisionConfig;
	}
}