using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Pooling.Identities
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