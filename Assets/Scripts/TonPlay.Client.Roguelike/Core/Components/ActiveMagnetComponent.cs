using System.Collections.Generic;
using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ActiveMagnetComponent
	{
		public float MagnetRadius;
		public float TimeLeft;
		public SimpleIntHashSet ExcludeEntityIds;
	}
}