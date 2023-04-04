using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemsConfigProvider
	{
		IInventoryItemConfig Get(string id);
	}
}