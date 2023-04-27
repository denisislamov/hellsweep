using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardItemContext : ScreenContext, IRewardItemContext
	{
		public Sprite Icon { get; }
		public Material GradientMaterial { get; }
		public int Count { get; }
		
		public RewardItemContext(Sprite icon, Material gradientMaterial, int count)
		{
			Icon = icon;
			GradientMaterial = gradientMaterial;
			Count = count;
		}
	}
}