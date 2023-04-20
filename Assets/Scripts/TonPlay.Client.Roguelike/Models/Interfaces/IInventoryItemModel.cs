using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IInventoryItemModel : IModel<InventoryItemData>
	{
		IReadOnlyReactiveProperty<string> Id { get; }
		
		IReadOnlyReactiveProperty<string> ItemId { get; }

		IReadOnlyReactiveProperty<string> DetailId { get; }
	}
}