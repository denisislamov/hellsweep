using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(fileName = nameof(LookAtTargetEnemyPropertyConfig), menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(LookAtTargetEnemyPropertyConfig))]
	public class LookAtTargetEnemyPropertyConfig : EnemyPropertyConfig, ILookAtTargetEnemyPropertyConfig
	{
	}
}