using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemsConfigUpdater
	{
		void Update(string id, ItemsGetResponse.Item item);
	}
}