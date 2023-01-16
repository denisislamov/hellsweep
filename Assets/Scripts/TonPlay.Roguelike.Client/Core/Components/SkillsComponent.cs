using System.Collections.Generic;
using TonPlay.Roguelike.Client.Core.Skills;

namespace TonPlay.Roguelike.Client.Core.Components
{
	public struct SkillsComponent
	{
		public Dictionary<SkillName, int> Levels;
	}
}