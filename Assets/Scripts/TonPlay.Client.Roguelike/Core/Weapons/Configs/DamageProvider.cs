using System;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[Serializable]
	public class DamageProvider : IDamageProvider
	{
		public DamageSource damageSource;

		public float damage;

		public float rate;

		private string _damageSource;

		private float _damageMultiplier = 1f;

		public string DamageSource => string.IsNullOrEmpty(_damageSource) ?
			_damageSource = damageSource.ToString() :
			_damageSource;

		public float Damage => damage;
		public float Rate => rate;
		public float DamageMultiplier
		{
			get => _damageMultiplier;
			set => _damageMultiplier = value;
		}
	}
}