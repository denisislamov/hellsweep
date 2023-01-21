using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ActiveMagnetComponent
	{
		public float MagnetRadius;
		public float TimeLeft;
		public HashSet<int> ExcludeEntityIds;
	}
}