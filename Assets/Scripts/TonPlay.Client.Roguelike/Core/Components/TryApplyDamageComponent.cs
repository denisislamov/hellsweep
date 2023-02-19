using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct TryApplyDamageComponent
	{
		public IDamageProvider DamageProvider;
		public int VictimEntityId;
	}
}