using TonPlay.Client.Roguelike.Models;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemConfig
	{
		string Id { get; }
		string Name { get; }
		RarityName Rarity { get; }
	}
}