namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces
{
	public interface IRotateMovementInTargetDirectionWhenDistanceExceededEnemyPropertyConfig : IEnemyPropertyConfig
	{
		float Distance { get; }
	}
}