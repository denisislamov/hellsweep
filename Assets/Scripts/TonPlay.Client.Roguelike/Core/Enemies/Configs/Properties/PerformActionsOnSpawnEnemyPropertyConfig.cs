using TonPlay.Client.Roguelike.Core.Actions;
using TonPlay.Client.Roguelike.Core.Actions.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(
		fileName = nameof(PerformActionsOnSpawnEnemyPropertyConfig), 
		menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(PerformActionsOnSpawnEnemyPropertyConfig))]
	public class PerformActionsOnSpawnEnemyPropertyConfig : EnemyPropertyConfig, IPerformActionsOnSpawnEnemyPropertyConfig
	{
		[SerializeField]
		private ScriptableAction[] _actions;
		
		public IAction[] Actions => _actions;
	}
}