using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces
{
	public interface ICollectableConfigProvider
	{
		IEnumerable<ICollectableConfig> AllCollectables { get; }

		ICollectableConfig Get(string id);
	}
}