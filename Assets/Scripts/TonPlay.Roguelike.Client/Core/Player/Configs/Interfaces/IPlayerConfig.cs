using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;

namespace TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces
{
	public interface IPlayerConfig
	{
		public string Id { get; }
		
		public PlayerView Prefab { get; }
		
		public ICollisionAreaConfig CollisionAreaConfig { get; }
		
		int StartHealth { get; }
	}
}