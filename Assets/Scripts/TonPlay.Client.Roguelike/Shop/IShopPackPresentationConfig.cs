using UnityEngine;

namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopPackPresentationConfig
	{
		string Id { get; }
		
		string Title { get; }
		
		string Description { get; }
		
		Color MainColor { get; }
		
		Material BackgroundGradientMaterial { get; }
	}
}