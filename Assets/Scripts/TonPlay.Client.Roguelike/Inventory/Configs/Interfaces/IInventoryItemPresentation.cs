using UnityEngine;

namespace TonPlay.Client.Roguelike.Inventory.Configs.Interfaces
{
	public interface IInventoryItemPresentation
	{
		Sprite Icon { get; }
		
		string Description { get; }
		
		string Title { get; }
		
		string GradeDescription { get; }
	}
}