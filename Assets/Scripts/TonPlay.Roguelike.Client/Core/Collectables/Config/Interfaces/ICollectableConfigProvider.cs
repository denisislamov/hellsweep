using System.Collections.Generic;

namespace TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces
{
	public interface ICollectableConfigProvider
	{
		IEnumerable<ICollectableConfig> AllCollectables { get; }

		ICollectableConfig Get(string id);
	}
}