using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface IWeaponConfig
	{
		string ItemId { get; }

		SkillName SkillName { get; }
	}
}