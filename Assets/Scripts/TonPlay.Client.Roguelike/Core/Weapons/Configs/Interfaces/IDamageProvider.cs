namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface IDamageProvider
	{
		string DamageSource { get; }

		float Damage { get; }

		float Rate { get; }
	}
}