using TonPlay.Client.Roguelike.Models;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemPresentationProvider
	{
		Sprite DefaultItemIcon { get; }
		
		void GetColors(RarityName rarityName, out Color mainColor, out Material backgroundGradient);

		Sprite GetSlotIcon(SlotName slotName);
		
		IInventoryItemPresentation GetItemPresentation(string itemId);
	}
}