using TonPlay.Roguelike.Client.Core.Player.Views;

namespace TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces
{
	public interface IPlayerSpawnConfig
	{
		public string Id { get; }
		
		public PlayerView Prefab { get; }
		
		int StartHealth { get; }
	}
}