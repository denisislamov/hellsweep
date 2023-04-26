using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade
{
	public class InventoryItemGradeDescriptionContext : ScreenContext, IInventoryItemGradeDescriptionContext
	{
		public string ItemId { get; }
		public RarityName UserItemRarityName { get; }
		public RarityName GradeDescriptionRarityName { get; }
		
		public InventoryItemGradeDescriptionContext(string itemId, RarityName userItemRarityName, RarityName gradeDescriptionRarityName)
		{
			ItemId = itemId;
			UserItemRarityName = userItemRarityName;
			GradeDescriptionRarityName = gradeDescriptionRarityName;
		}
	}
}