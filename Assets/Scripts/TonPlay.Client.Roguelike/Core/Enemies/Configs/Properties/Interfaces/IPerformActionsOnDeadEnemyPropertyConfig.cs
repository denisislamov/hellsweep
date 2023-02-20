using TonPlay.Client.Roguelike.Core.Actions.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces
{
	public interface IPerformActionsOnDeadEnemyPropertyConfig : IEnemyPropertyConfig
	{
		IAction[] Actions { get; }
	}
}