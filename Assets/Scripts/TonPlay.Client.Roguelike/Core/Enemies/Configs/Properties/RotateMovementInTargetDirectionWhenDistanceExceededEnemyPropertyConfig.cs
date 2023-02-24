using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(
		fileName = nameof(RotateMovementInTargetDirectionWhenDistanceExceededEnemyPropertyConfig),
		menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(RotateMovementInTargetDirectionWhenDistanceExceededEnemyPropertyConfig))]
	public class RotateMovementInTargetDirectionWhenDistanceExceededEnemyPropertyConfig : EnemyPropertyConfig, IRotateMovementInTargetDirectionWhenDistanceExceededEnemyPropertyConfig
	{
		[SerializeField]
		private float _distance;

		public float Distance => _distance;
	}
}