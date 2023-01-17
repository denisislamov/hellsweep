using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Components
{
	public struct WeaponComponent
	{
		public WeaponFireType FireType;
		public float FireDelay;
		public int OwnerEntityId;
		public string WeaponConfigId;
		public IViewPoolIdentity ProjectileIdentity;
	}
}