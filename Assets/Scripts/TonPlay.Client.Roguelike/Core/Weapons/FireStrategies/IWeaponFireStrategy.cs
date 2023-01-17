using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Weapons.FireStrategies
{
	public interface IWeaponFireStrategy
	{
		void Fire(ref WeaponComponent weaponComponent);
	}
}