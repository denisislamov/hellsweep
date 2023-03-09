using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces
{
	public interface ICollectableConfigProvider
	{
		ICollectableConfig InitialExperienceCollectableForFirstLevel { get; }

		IEnumerable<ICollectableConfig> AllCollectables { get; }

		ICollectableConfig Get(string id);
	}
}