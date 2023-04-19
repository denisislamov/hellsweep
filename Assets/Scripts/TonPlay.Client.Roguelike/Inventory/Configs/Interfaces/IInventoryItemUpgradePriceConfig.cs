namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemUpgradePriceConfig
	{
		string Id { get; }
		long Coins { get; }
		ushort Blueprints { get; }
	}
}