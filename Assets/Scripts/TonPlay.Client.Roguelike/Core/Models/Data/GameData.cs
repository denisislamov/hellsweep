namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class GameData
	{
		public float GameTime { get; set; }
		
		public int DeadEnemies { get; set; }
		
		public PlayerData PlayerData { get; set; }
		
		public bool Paused { get; set; }
	}
}