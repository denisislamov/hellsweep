using System;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
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
		
		public IDamageProvider AddDamageValue(float damage)
		{
			this.damage += damage;
			return this;
		}

		public IDamageProvider Clone() => CloneInternal();

		public DamageProvider CloneInternal()
		{
			return new DamageProvider()
			{
				damageSource = damageSource,
				damage = damage,
				rate = rate,
				_damageSource = _damageSource,
				_damageMultiplier = _damageMultiplier
			};
		}
	}
}