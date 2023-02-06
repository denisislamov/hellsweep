using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Roguelike.Client.Core.Skills;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct SkillsComponent
	{
		public Dictionary<SkillName, int> Levels;
	}
}