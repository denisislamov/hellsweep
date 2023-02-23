namespace TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces
{
	public interface IMagnetCollectableConfig : ICollectableConfig
	{
		float MagnetRadius { get; }

		float TimeToStart { get; }

		float ActiveTime { get; }
	}
}