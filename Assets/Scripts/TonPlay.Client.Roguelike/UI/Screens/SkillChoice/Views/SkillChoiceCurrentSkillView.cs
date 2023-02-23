using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views
{
	public class SkillChoiceCurrentSkillView : View, ISkillChoiceCurrentSkillView
	{
		[SerializeField]
		private Image _iconImage;

		public void SetColor(Color color)
		{
			_iconImage.color = color;
		}

		public void SetIcon(Sprite icon)
		{
			_iconImage.sprite = icon;
		}
	}
}