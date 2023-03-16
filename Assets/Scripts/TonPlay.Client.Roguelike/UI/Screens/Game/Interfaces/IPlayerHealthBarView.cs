using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Interfaces
{
	public interface IPlayerHealthBarView : IProgressBarView
	{
		void SetPosition(Vector2 position);
	}
}