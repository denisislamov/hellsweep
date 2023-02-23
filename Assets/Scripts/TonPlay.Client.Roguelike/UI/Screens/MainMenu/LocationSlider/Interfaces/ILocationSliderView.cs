using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider.Interfaces
{
	public interface ILocationSliderView : IView
	{
		IButtonView LeftButton { get; }

		IButtonView RightButton { get; }

		void SetTitleText(string text);

		void SetSubtitleText(string text);

		void SetIcon(Sprite sprite);
	}
}