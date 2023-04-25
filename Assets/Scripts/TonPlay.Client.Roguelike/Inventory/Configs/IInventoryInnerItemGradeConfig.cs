using TonPlay.Client.Roguelike.Models;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public interface IInventoryInnerItemGradeConfig
	{
		string ItemId { get; }
		string InnerItemId { get; }
		RarityName RarityName { get; }
	}
}