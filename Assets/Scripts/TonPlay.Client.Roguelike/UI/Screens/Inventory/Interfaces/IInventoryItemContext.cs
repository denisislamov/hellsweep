using System;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models;
using UniRx;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventoryItemContext : IScreenContext
	{
		IReadOnlyReactiveProperty<string> UserItemId { get; }
		string Name { get; }
		Sprite Icon { get; }
		Sprite SlotIcon { get; }
		Color MainColor { get; }
		IReadOnlyReactiveProperty<bool> IsEquipped { get; }
		IReadOnlyReactiveProperty<MergeStates> MergeState { get; }
		Material BackgroundGradientMaterial { get; }
		Action ClickCallback { get; }
	}
}