using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public class InventoryItemUpgradePriceConfig : IInventoryItemUpgradePriceConfig
	{
		public string Id { get; }
		public long Coins { get; }
		public ushort Blueprints { get; }

		public InventoryItemUpgradePriceConfig(
			string id, 
			long coins, 
			ushort blueprints)
		{
			Id = id;
			Coins = coins;
			Blueprints = blueprints;
		}
	}
}