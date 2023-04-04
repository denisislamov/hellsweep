using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemContext : ScreenContext, IInventoryItemContext
	{
		public string Id { get; }
		
		public string Name { get; }
		
		public Sprite Icon { get; }
		
		public Color MainColor { get; }
		
		public Gradient BackgroundGradient { get; }

		public InventoryItemContext(string id, Sprite icon, Color mainColor, Gradient backgroundGradient, string name)
		{
			Id = id;
			Icon = icon;
			MainColor = mainColor;
			BackgroundGradient = backgroundGradient;
			Name = name;
		}
	}
}