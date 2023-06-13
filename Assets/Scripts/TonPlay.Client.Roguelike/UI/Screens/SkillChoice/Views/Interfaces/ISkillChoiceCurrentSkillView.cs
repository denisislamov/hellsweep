using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces
{
	public interface ISkillChoiceCurrentSkillView : IView
	{
		void SetColor(Color color);

		void SetIcon(Sprite icon);

		public void SetBackgroundEmptySprite();

		public void SetBackgroundFilledSprite();
	}
}