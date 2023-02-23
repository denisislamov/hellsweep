using TonPlay.Client.Roguelike.Core.Actions;
using TonPlay.Client.Roguelike.Core.Actions.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties
{
	[CreateAssetMenu(
		fileName = nameof(PerformActionsOnDeadEnemyPropertyConfig),
		menuName = AssetMenuConstants.ENEMIES_PROPERTIES_CONFIGS + nameof(PerformActionsOnDeadEnemyPropertyConfig))]
	public class PerformActionsOnDeadEnemyPropertyConfig : EnemyPropertyConfig, IPerformActionsOnDeadEnemyPropertyConfig
	{
		[SerializeField]
		private ScriptableAction[] _actions;

		public IAction[] Actions => _actions;
	}
}