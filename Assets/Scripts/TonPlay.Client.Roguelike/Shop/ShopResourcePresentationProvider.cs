using System;
using System.Linq;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Shop
{
	[CreateAssetMenu(fileName = nameof(ShopResourcePresentationProvider), menuName = AssetMenuConstants.SHOP_UI_CONFIGS + nameof(ShopResourcePresentationProvider))]
	public class ShopResourcePresentationProvider : ScriptableObject, IShopResourcePresentationProvider
	{
		[SerializeField]
		private ShopResourcePresentationConfig[] _configs;
		
		public IShopResourcePresentationConfig Get(string packId)
		{
			return _configs.FirstOrDefault(_ => _.Id == packId);
		}
		
		[Serializable]
		private class ShopResourcePresentationConfig : IShopResourcePresentationConfig
		{
			[SerializeField]
			private string _id;
			
			[SerializeField]
			private string _title;
			
			[SerializeField]
			private string _description;
			
			[SerializeField]
			private Material _backgroundGradientMaterial;
			
			[SerializeField]
			private Sprite _icon;

			public string Id => _id;
			public string Title => _title;
			public string Description => _description;
			
			public Material BackgroundGradientMaterial => _backgroundGradientMaterial;
			public Sprite Icon => _icon;
		}
	}
}