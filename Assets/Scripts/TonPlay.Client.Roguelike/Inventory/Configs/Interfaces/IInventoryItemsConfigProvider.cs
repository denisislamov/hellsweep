namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemsConfigProvider
	{
		IInventoryItemConfig Get(string id);

		IInventoryItemUpgradePriceConfig GetUpgradePrice(ushort level);
	}
}