using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventoryItemContext : IScreenContext
	{
		string Id { get; }
		string Name { get; }
		Sprite Icon { get; }
		Color MainColor { get; }
		Gradient BackgroundGradient { get; }
	}
}