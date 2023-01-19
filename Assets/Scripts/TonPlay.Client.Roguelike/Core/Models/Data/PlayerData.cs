namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class PlayerData
	{
		public float Health { get; set; }
		public float MaxHealth { get; set; }
		public float Experience { get; set; }
		public float MaxExperience { get; set; }
		public SkillsData SkillsData { get; set; }
		public MatchProfileGainData MatchProfileGainModel { get; set; }
	}
}