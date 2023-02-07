using System;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[Serializable]
	public class DamageProvider : IDamageProvider
	{
		public float damage;

		public float Damage => damage;
	}
}