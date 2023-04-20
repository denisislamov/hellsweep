using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

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
		private InventoryItemPresentation[] _itemPresentations;

		[SerializeField]
		private Sprite _defaultIcon;
		
		[SerializeField]
		private SlotIconSet[] _slotIcons;
		
		[SerializeField]
		private Sprite _defaultSlotIcon;

		private Dictionary<RarityName, ColorSet> _colorSetsByRarityMap;
		private Dictionary<RarityName, ColorSet> ColorSetsByRarity => _colorSetsByRarityMap ??= _colorSetsByRarity.ToDictionary(_ => _.Rarity, _ => _);

		
		private Dictionary<string, InventoryItemPresentation> _presentationsMap;
		private Dictionary<string, InventoryItemPresentation> ItemsPresentations => _presentationsMap ??= _itemPresentations.ToDictionary(_ => _.ItemId, _ => _);
		
		private Dictionary<SlotName, Sprite> _slotIconsMap;
		private Dictionary<SlotName, Sprite> SlotIcons => _slotIconsMap ??= _slotIcons.ToDictionary(_ => _.SlotName, _ => _.Sprite);

		public Sprite DefaultItemIcon => _defaultIcon;
		
		public void GetColors(RarityName rarityName, out Color mainColor, out Material backgroundGradient)
		{
			var colorSet = ColorSetsByRarity.ContainsKey(rarityName) ? ColorSetsByRarity[rarityName] : _defaultColorSet;
			
			mainColor = colorSet.MainColor;
			backgroundGradient = colorSet.Gradient;
		}
		
		public Sprite GetSlotIcon(SlotName slotName)
		{
			return SlotIcons.ContainsKey(slotName) ? SlotIcons[slotName] : _defaultSlotIcon;
		}
		
		public IInventoryItemPresentation GetItemPresentation(string itemId)
		{
			return !string.IsNullOrWhiteSpace(itemId) && ItemsPresentations.ContainsKey(itemId) ? ItemsPresentations[itemId] : null;
		}
	}
	
	[Serializable]
	internal class ColorSet
	{
		public RarityName Rarity;
		public Color MainColor;
		public Material Gradient;
	}
	
	[Serializable]
	internal class SlotIconSet
	{
		public SlotName SlotName;
		public Sprite Sprite;
	}
	
	[Serializable]
	internal class InventoryItemPresentation : IInventoryItemPresentation
	{
		[SerializeField]
		private string _title;
		
		[SerializeField]
		private string _itemId;
		
		[SerializeField]
		private Sprite _icon;

		[SerializeField]
		private string _description;
		
		public string ItemId => _itemId;
		public Sprite Icon => _icon;
		public string Description => _description;
		public string Title => _title;
	}
}