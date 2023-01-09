using TonPlay.Roguelike.Client.Core.Player.Views;
using TonPlay.Roguelike.Client.Core.Weapons.Views;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces
{
	public interface IWeaponConfig
	{
		string Id { get; }
		
		WeaponView Prefab { get; }
		
		float FireDelay { get; }
		
		WeaponFireType FireType { get; }

		IProjectileConfig GetProjectileConfig();
	}
}