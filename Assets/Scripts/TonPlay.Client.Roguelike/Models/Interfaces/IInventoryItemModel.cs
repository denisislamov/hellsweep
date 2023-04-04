using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IInventoryItemModel : IModel<InventoryItemData>
	{
		public IReadOnlyReactiveProperty<string> Id { get; }
		
		public IReadOnlyReactiveProperty<string> Name { get; }
		
		public IReadOnlyReactiveProperty<RarityName> Rarity { get; }
	}
}