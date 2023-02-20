using TonPlay.Client.Roguelike.Core.Actions.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces
{
	public interface IPerformActionsOnSpawnEnemyPropertyConfig : IEnemyPropertyConfig
	{
		IAction[] Actions { get; }
	}
}