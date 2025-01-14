using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Skills;

namespace TonPlay.Client.Roguelike.Core.Models.Data
{
	public class SkillsData
	{
		public Dictionary<SkillName, int> SkillLevels { get; set; }
		public int Level { get; set; }
	}
}