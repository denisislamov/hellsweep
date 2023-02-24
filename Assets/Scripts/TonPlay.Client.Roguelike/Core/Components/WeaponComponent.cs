using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
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