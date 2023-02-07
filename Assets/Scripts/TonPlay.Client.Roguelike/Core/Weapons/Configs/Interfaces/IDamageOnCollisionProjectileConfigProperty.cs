using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface IDamageOnCollisionProjectileConfigProperty : IProjectileConfigProperty
	{
		IDamageProvider DamageProvider { get; }
	}
}