using TonPlay.Client.Roguelike.Models;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemPresentationProvider
	{
		void GetColors(RarityName rarityName, out Color mainColor, out Gradient backgroundGradient);

		Sprite GetIcon(string itemId);
	}
}