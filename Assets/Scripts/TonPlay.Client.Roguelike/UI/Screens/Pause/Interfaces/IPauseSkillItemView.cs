using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces
{
	public interface IPauseSkillItemView : IView
	{
		void SetIcon(Sprite icon);

		void SetCurrentLevel(int level);
		
		void SetMaxLevel(int level);
		
		void SetColor(Color color);
	}
}