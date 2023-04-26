using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces
{
	public interface IInventoryItemGradeDescriptionContext : IScreenContext
	{
		string ItemId { get; }
		RarityName UserItemRarityName { get; }
		RarityName GradeDescriptionRarityName { get; }
	}
}