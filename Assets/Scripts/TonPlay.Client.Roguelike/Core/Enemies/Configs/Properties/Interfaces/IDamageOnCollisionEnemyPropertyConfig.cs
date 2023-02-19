using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces
{
	public interface IDamageOnCollisionEnemyPropertyConfig : IEnemyPropertyConfig
	{
		IDamageProvider DamageProvider { get; }
	}
}