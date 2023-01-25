using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct PrepareToExplodeCollectedBombsComponent
	{
		public List<(float, IBombCollectableConfig)> Bombs;
	}
}