using TonPlay.Roguelike.Client.Core.Movement.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces
{
	public interface IMoveOnPlayerEnemyPropertyConfig : IEnemyPropertyConfig
	{
		IMovementConfig MovementConfig { get; }
	}
}