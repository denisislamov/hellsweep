using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models;

namespace TonPlay.Client.Roguelike.Inventory.Configs
{
	public interface IInventoryInnerItemConfig
	{
		string InnerItemId { get; }

		IReadOnlyList<IInventoryInnerItemGradeConfig> Grades { get; }
		
		IInventoryInnerItemGradeConfig GetGradeConfig(RarityName rarityName);
	}
}