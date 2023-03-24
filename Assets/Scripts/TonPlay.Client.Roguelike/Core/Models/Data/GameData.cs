using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class GameData : IData
	{
		public double GameTimeInSeconds { get; set; }

		public int DeadEnemies { get; set; }

		public PlayerData PlayerData { get; set; }

		public bool Paused { get; set; }
		
		public int DebugEnemyMovementToEachOtherCollisionCount { get; set; }
	}
}