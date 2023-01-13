using TMPro;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.Screens.Game.Timer.Views
{
	public class TimerView : View, ITimerView
	{
		[SerializeField]
		private TextMeshProUGUI _hours;
		
		[SerializeField]
		private TextMeshProUGUI _minutes;
		
		[SerializeField]
		private TextMeshProUGUI _seconds;
		
		[SerializeField]
		private TextMeshProUGUI _hoursToMinutesDelimiter;
		
		[SerializeField]
		private TextMeshProUGUI _minutesToSecondsDelimiter;

		public void SetSecondsActiveState(bool state)
		{
			_seconds.gameObject.SetActive(state);
		}
		
		public void SetMinutesActiveState(bool state)
		{
			_minutes.gameObject.SetActive(state);
		}
		
		public void SetHoursActiveState(bool state)
		{
			_hours.gameObject.SetActive(state);
		}
		
		public void SetSecondsText(string secondsText)
		{
			_seconds.SetText(secondsText);
		}
		
		public void SetMinutesText(string minutesText)
		{
			_minutes.SetText(minutesText);
		}
		
		public void SetHoursText(string hoursText)
		{
			_hours.SetText(hoursText);
		}
		
		public void SetHoursToMinutesDelimiterActiveState(bool state)
		{
			_hoursToMinutesDelimiter.gameObject.SetActive(state);
		}
		
		public void SetMinutesToSecondsDelimiterActiveState(bool state)
		{
			_minutesToSecondsDelimiter.gameObject.SetActive(state);
		}
	}
}