using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public class InventoryItemDetailConfig : IInventoryItemDetailConfig
	{
		public string Feature { get; set; }
		public ushort Level { get; set; }
		public uint Value { get; set; }
		public IInventoryItemDetailConfig Next { get; set; }
	}
}