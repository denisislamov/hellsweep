using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Rewards.Interfaces
{
	public interface IRewardItemContext : IScreenContext
	{
		Sprite Icon { get; }
		
		Material GradientMaterial { get; }
		
		int Count { get; }
	}
}