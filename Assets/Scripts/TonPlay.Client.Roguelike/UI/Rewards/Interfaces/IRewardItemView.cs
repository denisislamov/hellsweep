using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Rewards.Interfaces
{
	public interface IRewardItemView : ICollectionItemView
	{
		void SetIcon(Sprite icon);

		void SetBackgroundGradientMaterial(Material material);

		void SetCountText(string text);

		void SetCountActiveState(bool state);
	}
}