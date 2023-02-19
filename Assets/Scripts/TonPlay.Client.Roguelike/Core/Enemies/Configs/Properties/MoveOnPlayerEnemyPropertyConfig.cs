using System;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Roguelike.Client.Core.Movement;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(fileName = nameof(MoveOnPlayerEnemyPropertyConfig), menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(MoveOnPlayerEnemyPropertyConfig))]
	public class MoveOnPlayerEnemyPropertyConfig : EnemyPropertyConfig, IMoveOnPlayerEnemyPropertyConfig
	{
		[SerializeField]
		private MovementConfig _movementConfig;
		
		public IMovementConfig MovementConfig => _movementConfig;
	}
}