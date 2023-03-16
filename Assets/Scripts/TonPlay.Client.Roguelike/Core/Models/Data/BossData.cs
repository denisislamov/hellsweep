using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class BossData : IData
	{
		public bool Exists { get; set; }
		public float Health { get; set; }
		public float MaxHealth { get; set; }
	}
}