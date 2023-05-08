using UnityEngine;

namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopResourcePresentationConfig
	{
		string Id { get; }
		
		string Title { get; }
		
		string Description { get; }
		
		Material BackgroundGradientMaterial { get; }
		
		Sprite Icon { get; }
	}
}