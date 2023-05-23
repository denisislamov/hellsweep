using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components.Enemies
{
	public struct ShootProjectileAtTargetComponent
	{
		public IProjectileConfig ProjectileConfig;
		public IViewPoolIdentity ProjectileIdentity;
		public int Layer;
		public float Rate;
		public float TimeLeft;
		public float MinDistanceTargetToShoot;
		public float MaxDistanceTargetToShoot;
		public float FieldOfView;
		public int Quantity;
	}
}