using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces
{
	public interface IShopRewardItemCollectionContext : IScreenContext
	{
		IReadOnlyList<IShopRewardItemContext> Rewards { get; }
	}
}