using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Player.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace TonPlay.Client.Roguelike.Core.Player.Configs
{
	[Serializable]
	public class SkinConfig : ISkinConfig
	{
		[SerializeField]
		private string _id;

		[SerializeField]
		private List<WeaponSlotInventorySpriteSet> _weaponSlotInventorySpriteSets;

		public string Id => _id;

		public RectTransform GetInventorySpriteForWeaponItemId(string itemId)
		{
			return _weaponSlotInventorySpriteSets.FirstOrDefault(_ => _.ItemIds.Any(__ => __ == itemId))?.InventoryViewPrefab;
		} 
		
		public PlayerView GetPlayerViewForWeaponItemId(string itemId)
		{
			return _weaponSlotInventorySpriteSets.FirstOrDefault(_ => _.ItemIds.Any(__ => __ == itemId))?.PlayerViewPrefab;
		} 

		[Serializable]
		internal class WeaponSlotInventorySpriteSet
		{
			[SerializeField]
			private string[] _itemIds;

			[SerializeField]
			private RectTransform _inventoryViewPrefab;

			[SerializeField]
			private PlayerView _playerViewPrefab;
			
			public string[] ItemIds => _itemIds;
			public RectTransform InventoryViewPrefab => _inventoryViewPrefab;
			public PlayerView PlayerViewPrefab => _playerViewPrefab;
		}
	}
}