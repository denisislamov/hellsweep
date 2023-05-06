using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	[CreateAssetMenu(fileName = nameof(InventoryItemsConfigProvider), menuName = AssetMenuConstants.CONFIGS + nameof(InventoryItemsConfigProvider))]
	public class InventoryItemsConfigProvider : ScriptableObject, IInventoryItemsConfigProvider
	{
		[SerializeField]
		private List<Item> _items;
		public DictionaryExt<string, IInventoryItemConfig> ConfigsMap { get; } = new DictionaryExt<string, IInventoryItemConfig>();
		public DictionaryExt<ushort, IInventoryItemUpgradePriceConfig> UpgradePricesMap { get; } = new DictionaryExt<ushort, IInventoryItemUpgradePriceConfig>();

		public readonly Dictionary<string, ItemsGetResponse.Item> NextRarityMap = new Dictionary<string, ItemsGetResponse.Item>();

		public IReadOnlyDictionary<string, IInventoryInnerItemConfig> InnerItemMap =>
			_itemsMap ??= _items.ToDictionary(
				_ => _.InnerItemId,
				_ => (IInventoryInnerItemConfig)_);

		private Dictionary<string, IInventoryInnerItemConfig> _itemsMap;

		public IReadOnlyDictionary<string, IInventoryInnerItemConfig> InnerItemIdMapByItemId =>
			_itemsByItemIdMap ??= _items
								 .SelectMany(_ => _.Grades)
								 .ToDictionary(
									  _ => _.ItemId,
									  _ => InnerItemMap[_.InnerItemId]);

		private Dictionary<string, IInventoryInnerItemConfig> _itemsByItemIdMap;
		
		// TODO add map for next rarity or something similar
		
		public IInventoryItemConfig Get(string id) => !string.IsNullOrWhiteSpace(id) && ConfigsMap.ContainsKey(id)
			? ConfigsMap[id]
			: default;

		public IInventoryItemUpgradePriceConfig GetUpgradePrice(ushort level) => UpgradePricesMap.ContainsKey(level)
			? UpgradePricesMap[level]
			: default;

		public IInventoryInnerItemConfig GetInnerItemConfig(string itemId) => 
			!string.IsNullOrEmpty(itemId) && InnerItemIdMapByItemId.ContainsKey(itemId) ? InnerItemIdMapByItemId[itemId] : default;

		public Dictionary<string, ItemsGetResponse.Item> GetNextRarityMap() => NextRarityMap;
		
		[Serializable]
		public class Item : IInventoryInnerItemConfig
		{
			[SerializeField]
			private string _innerItemId;

			[SerializeField]
			private List<ItemGrade> _itemGrades;

			public string InnerItemId => _innerItemId;

			private bool _initialized;
			public IReadOnlyList<IInventoryInnerItemGradeConfig> Grades
			{
				get
				{
					if (!_initialized)
					{
						_initialized = true;
						_itemGrades.ForEach(_ => _.InnerItemId = _innerItemId);
					}
					return _itemGrades;
				}
			}

			public IReadOnlyDictionary<string, IInventoryInnerItemGradeConfig> GradesMapByItemId =>
				_gradesMapByItemId ??= _itemGrades.ToDictionary(
					_ => _.ItemId,
					_ =>
					{
						_.InnerItemId = _innerItemId;
						return (IInventoryInnerItemGradeConfig)_;
					});

			public IReadOnlyDictionary<RarityName, IInventoryInnerItemGradeConfig> GradesMapByRarity =>
				_gradesMapByRarity ??= _itemGrades.ToDictionary(
					_ => _.RarityName,
					_ =>
					{
						_.InnerItemId = _innerItemId;
						return (IInventoryInnerItemGradeConfig)_;
					});

			private Dictionary<string, IInventoryInnerItemGradeConfig> _gradesMapByItemId;
			private Dictionary<RarityName, IInventoryInnerItemGradeConfig> _gradesMapByRarity;

			public IInventoryInnerItemGradeConfig GetGradeConfig(RarityName rarityName) =>
				GradesMapByRarity.ContainsKey(rarityName) ? GradesMapByRarity[rarityName] : null;

			public IInventoryInnerItemGradeConfig GetGradeConfig(string itemId) =>
				!string.IsNullOrEmpty(itemId) && GradesMapByItemId.ContainsKey(itemId) ? GradesMapByItemId[itemId] : null;

			[Serializable]
			public class ItemGrade : IInventoryInnerItemGradeConfig
			{
				[SerializeField]
				private string _itemId;

				[SerializeField]
				private RarityName _rarityName;

				public string ItemId => _itemId;
				public string InnerItemId { get; set; }
				public RarityName RarityName => _rarityName;
			}
		}
	}
}