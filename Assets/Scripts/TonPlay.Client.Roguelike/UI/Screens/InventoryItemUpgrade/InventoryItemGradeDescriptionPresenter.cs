using System;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Network.Interfaces;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade
{
	internal class InventoryItemGradeDescriptionPresenter : Presenter<IInventoryItemGradeDescriptionView, IInventoryItemGradeDescriptionContext>
	{
		private readonly IInventoryItemsConfigProvider _inventoryItemsConfigProvider;
		private readonly IInventoryItemPresentationProvider _inventoryItemPresentationProvider;

		public InventoryItemGradeDescriptionPresenter(
			IInventoryItemGradeDescriptionView view,
			IInventoryItemGradeDescriptionContext context,
			IInventoryItemsConfigProvider inventoryItemsConfigProvider,
			IInventoryItemPresentationProvider inventoryItemPresentationProvider)
			: base(view, context)
		{
			_inventoryItemsConfigProvider = inventoryItemsConfigProvider;
			_inventoryItemPresentationProvider = inventoryItemPresentationProvider;

			InitView();
		}

		private void InitView()
		{
			var innerItemConfig = _inventoryItemsConfigProvider.GetInnerItemConfig(Context.ItemId);
			var gradeConfig = innerItemConfig.GetGradeConfig(Context.GradeDescriptionRarityName);
			var innerItemPresentationConfig = _inventoryItemPresentationProvider.GetItemPresentation(gradeConfig.ItemId);

			_inventoryItemPresentationProvider.GetColors(Context.GradeDescriptionRarityName, out var color, out var gradient);

			View.SetText(innerItemPresentationConfig.GradeDescription);
			View.SetUnlockedState(Context.UserItemRarityName >= gradeConfig.RarityName);
			View.SetIconBackgroundColor(color);
		}

		internal class Factory : PlaceholderFactory<IInventoryItemGradeDescriptionView, IInventoryItemGradeDescriptionContext, InventoryItemGradeDescriptionPresenter>
		{
		}
	}
}