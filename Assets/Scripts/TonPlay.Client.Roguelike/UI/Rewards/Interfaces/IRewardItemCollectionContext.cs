using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Rewards.Interfaces
{
	public interface IRewardItemCollectionContext : IScreenContext
	{
		IReadOnlyList<IRewardData> Rewards { get; }
	}
}