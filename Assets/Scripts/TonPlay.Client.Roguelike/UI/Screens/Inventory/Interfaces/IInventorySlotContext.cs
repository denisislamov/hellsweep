using System;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventorySlotContext : IScreenContext
	{
		ISlotModel SlotModel { get; }
		Action ClickCallback { get; }
	}
}