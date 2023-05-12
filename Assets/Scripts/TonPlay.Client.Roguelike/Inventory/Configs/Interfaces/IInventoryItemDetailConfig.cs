namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemDetailConfig
	{
		string Id { get; }
		ushort Level { get; }
		uint Value { get; }
		IInventoryItemDetailConfig Next { get; }
	}
}