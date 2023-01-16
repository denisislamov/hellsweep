namespace TonPlay.Roguelike.Client.Core.Movement.Interfaces
{
	public interface IMovementConfig
	{
		float StartSpeed { get; }
		
		float Acceleration { get; }
	}
}