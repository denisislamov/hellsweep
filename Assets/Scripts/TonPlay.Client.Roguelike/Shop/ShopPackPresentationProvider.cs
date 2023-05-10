using System;
using System.Linq;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Shop
{
	[CreateAssetMenu(fileName = nameof(ShopPackPresentationProvider), menuName = AssetMenuConstants.SHOP_UI_CONFIGS + nameof(ShopPackPresentationProvider))]
	public class ShopPackPresentationProvider : ScriptableObject, IShopPackPresentationProvider
	{
		[SerializeField]
		private ShopPackPresentationConfig[] _configs;
		
		[SerializeField]
		private ShopPackRewardPresentationConfig[] _rewardConfigs;
		
		public IShopPackPresentationConfig Get(string packId)
		{
			return _configs.FirstOrDefault(_ => _.Id == packId);
		}
		
		public IShopPackRewardPresentationConfig GetRewardPresentation(string rewardId)
		{
			return _rewardConfigs.FirstOrDefault(_ => _.Id == rewardId);
		}

		[Serializable]
		private class ShopPackPresentationConfig : IShopPackPresentationConfig
		{
			[SerializeField]
			private string _id;
			
			[SerializeField]
			private string _title;
			
			[SerializeField]
			private string _description;
			
			[SerializeField]
			private string _rarityText;
			
			[SerializeField]
			private Color _mainColor;
			
			[SerializeField]
			private Material _backgroundGradientMaterial;

			public string Id => _id;
			public string Title => _title;
			public string Description => _description;
			public string RarityText => _rarityText;

			public Color MainColor => _mainColor;
			
			public Material BackgroundGradientMaterial => _backgroundGradientMaterial;
		}
		
		[Serializable]
		private class ShopPackRewardPresentationConfig : IShopPackRewardPresentationConfig
		{
			[SerializeField]
			private string _id;
			
			[SerializeField]
			private Sprite _icon;
			
			[SerializeField]
			private Material _backgroundGradientMaterial;

			public string Id => _id;
			
			public Sprite Icon => _icon;
			
			public Material BackgroundGradientMaterial => _backgroundGradientMaterial;
		}
	}
}