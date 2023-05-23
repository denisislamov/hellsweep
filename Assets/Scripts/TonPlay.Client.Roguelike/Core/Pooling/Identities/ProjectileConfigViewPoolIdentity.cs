using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Pooling.Identities
{
	public class ProjectileConfigViewPoolIdentity : IViewPoolIdentity
	{
		public string Id { get; }

		public ProjectileConfigViewPoolIdentity(IProjectileConfig projectileConfig)
		{
			Id = string.Format("Projectile.{0}", projectileConfig.PrefabView.name);
		}
	}
}