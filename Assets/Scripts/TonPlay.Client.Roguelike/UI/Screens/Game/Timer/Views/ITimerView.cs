using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views
{
	public interface ITimerView : IView
	{
		void SetSecondsActiveState(bool state);
		
		void SetMinutesActiveState(bool state);
		
		void SetHoursActiveState(bool state);
		
		void SetSecondsText(string secondsText);
		
		void SetMinutesText(string minutesText);
		
		void SetHoursText(string hoursText);
		
		void SetHoursToMinutesDelimiterActiveState(bool state);
		
		void SetMinutesToSecondsDelimiterActiveState(bool state);
	}
}