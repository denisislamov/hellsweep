using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class MatchProfileGainData
	{
		public int Gold { get; set; }
		public float ProfileExperience { get; set; }
		
		public List<string> ChestsId { get; set; }
	}
}