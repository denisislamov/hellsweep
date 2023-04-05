using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	[CreateAssetMenu(fileName = nameof(InventoryItemPresentationProvider), menuName = AssetMenuConstants.CONFIGS + nameof(InventoryItemPresentationProvider))]
	public class InventoryItemPresentationProvider : ScriptableObject, IInventoryItemPresentationProvider
	{
		[SerializeField]
		private ColorSet[] _colorSetsByRarity;
		
		[SerializeField]
		private ColorSet _defaultColorSet;
		
		[SerializeField]
		private IconSet[] _icons;

		[SerializeField]
		private Sprite _defaultIcon;
		
		[SerializeField]
		private SlotIconSet[] _slotIcons;
		
		[SerializeField]
		private Sprite _defaultSlotIcon;

		private Dictionary<RarityName, ColorSet> _colorSetsByRarityMap;
		private Dictionary<RarityName, ColorSet> ColorSetsByRarity => _colorSetsByRarityMap ??= _colorSetsByRarity.ToDictionary(_ => _.Rarity, _ => _);

		
		private Dictionary<string, Sprite> _iconsMap;
		private Dictionary<string, Sprite> Icons => _iconsMap ??= _icons.ToDictionary(_ => _.Id, _ => _.Sprite);
		
		private Dictionary<SlotName, Sprite> _slotIconsMap;
		private Dictionary<SlotName, Sprite> SlotIcons => _slotIconsMap ??= _slotIcons.ToDictionary(_ => _.SlotName, _ => _.Sprite);

		
		public void GetColors(RarityName rarityName, out Color mainColor, out Gradient backgroundGradient)
		{
			var colorSet = ColorSetsByRarity.ContainsKey(rarityName) ? ColorSetsByRarity[rarityName] : _defaultColorSet;
			
			mainColor = colorSet.MainColor;
			backgroundGradient = colorSet.Gradient;
		}
		
		public Sprite GetIcon(string itemId)
		{
			return !string.IsNullOrWhiteSpace(itemId) && Icons.ContainsKey(itemId) ? Icons[itemId] : _defaultIcon;
		}
		
		public Sprite GetSlotIcon(SlotName slotName)
		{
			return SlotIcons.ContainsKey(slotName) ? SlotIcons[slotName] : _defaultSlotIcon;
		}
	}
	
	[Serializable]
	internal class ColorSet
	{
		public RarityName Rarity;
		public Color MainColor;
		public Gradient Gradient;
	}
	
	[Serializable]
	internal class IconSet
	{
		public string Id;
		public Sprite Sprite;
	}
	
	[Serializable]
	internal class SlotIconSet
	{
		public SlotName SlotName;
		public Sprite Sprite;
	}
}