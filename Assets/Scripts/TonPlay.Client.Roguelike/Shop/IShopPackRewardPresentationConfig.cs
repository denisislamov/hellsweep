using UnityEngine;

namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopPackRewardPresentationConfig
	{
		string Id { get; }
		
		Sprite Icon { get; }
		
		Material BackgroundGradientMaterial { get; }
	}
}