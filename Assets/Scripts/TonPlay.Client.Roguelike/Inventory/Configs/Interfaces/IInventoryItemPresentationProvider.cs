using TonPlay.Client.Roguelike.Core.Player.Configs;
using TonPlay.Client.Roguelike.Models;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemPresentationProvider
	{
		Sprite DefaultItemIcon { get; }
		
		void GetColors(RarityName rarityName, out Color mainColor, out Material backgroundGradient);

		Sprite GetSlotIcon(SlotName slotName);

		Sprite GetItemAttributeIcon(AttributeName attributeName);
		
		IInventoryItemPresentation GetItemPresentation(string itemId);
	}
}