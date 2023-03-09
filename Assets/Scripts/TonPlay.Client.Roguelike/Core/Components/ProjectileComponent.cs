using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ProjectileComponent
	{
		public IProjectileConfig Config;
		public int CreatorEntityId;
	}
}