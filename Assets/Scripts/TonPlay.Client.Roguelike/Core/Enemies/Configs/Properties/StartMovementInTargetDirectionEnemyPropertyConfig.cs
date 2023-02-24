using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(
		fileName = nameof(StartMovementInTargetDirectionEnemyPropertyConfig),
		menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(StartMovementInTargetDirectionEnemyPropertyConfig))]
	public class StartMovementInTargetDirectionEnemyPropertyConfig : EnemyPropertyConfig, IStartMovementInTargetDirectionEnemyPropertyConfig
	{
	}
}