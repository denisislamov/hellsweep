using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(
		fileName = nameof(DamageOnCollisionEnemyPropertyConfig),
		menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(DamageOnCollisionEnemyPropertyConfig))]
	public class DamageOnCollisionEnemyPropertyConfig : EnemyPropertyConfig, IDamageOnCollisionEnemyPropertyConfig
	{
		[SerializeField]
		private DamageProvider _damageProvider;

		public IDamageProvider DamageProvider => _damageProvider;
	}
}