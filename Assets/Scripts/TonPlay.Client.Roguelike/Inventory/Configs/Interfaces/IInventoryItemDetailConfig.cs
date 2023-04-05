namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemDetailConfig
	{
		string Feature { get; }
		ushort Level { get; }
		uint Value { get; }
	}
}