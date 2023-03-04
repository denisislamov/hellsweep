using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardItemContext : ScreenContext, IRewardItemContext
	{
		public Sprite Icon { get; }
		public int Count { get; }
		
		public RewardItemContext(Sprite icon, int count)
		{
			Icon = icon;
			Count = count;
		}
	}
}