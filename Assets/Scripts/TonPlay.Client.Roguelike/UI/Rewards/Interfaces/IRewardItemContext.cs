using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Rewards.Interfaces
{
	public interface IRewardItemContext : IScreenContext
	{
		Sprite Icon { get; }
		
		int Count { get; }
	}
}