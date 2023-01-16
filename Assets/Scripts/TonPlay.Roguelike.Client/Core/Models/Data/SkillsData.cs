using System.Collections.Generic;
using TonPlay.Roguelike.Client.Core.Skills;

namespace TonPlay.Roguelike.Client.Core.Models.Data
{
	public class SkillsData
	{
		public Dictionary<SkillName, int> SkillLevels { get; set; }
		public int Level { get; set; }
	}
}