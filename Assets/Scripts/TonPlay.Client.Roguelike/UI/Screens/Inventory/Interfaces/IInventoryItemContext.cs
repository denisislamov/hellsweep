using System;
using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventoryItemContext : IScreenContext
	{
		string Id { get; }
		string Name { get; }
		ushort Level { get; }
		Sprite Icon { get; }
		Sprite SlotIcon { get; }
		Color MainColor { get; }
		Gradient BackgroundGradient { get; }
		Action ClickCallback { get; }
	}
}