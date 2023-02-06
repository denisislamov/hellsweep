using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces
{
	public interface IWeaponConfigProvider
	{
		IWeaponConfig Get(string id);
	}
}