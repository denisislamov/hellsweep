using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Timer
{
	public class TimerPresenter : Presenter<ITimerView, ITimerContext>
	{
		private readonly string[] _timeStrings = new string[61];

		private IDisposable _subscription;

		public TimerPresenter(ITimerView view, ITimerContext context) : base(view, context)
		{
			Init();
			AddSubscription();
		}

		public override void Dispose()
		{
			_subscription?.Dispose();

			base.Dispose();
		}

		private void Init()
		{
			CreateTimeStrings();
		}

		private void CreateTimeStrings()
		{
			for (var i = 0; i <= 60; i++)
			{
				_timeStrings[i] = i.ToString("00");
			}
		}

		private void AddSubscription()
		{
			_subscription = Context.TimeInSeconds.Subscribe(UpdateView);
		}

		private void UpdateView(float seconds)
		{
			var timeSpan = TimeSpan.FromSeconds(seconds);

			View.SetMinutesText(_timeStrings[(int)timeSpan.TotalMinutes]);
			View.SetSecondsText(_timeStrings[timeSpan.Seconds]);

			View.SetHoursActiveState(false);
			View.SetMinutesActiveState(true);
			View.SetSecondsActiveState(true);

			View.SetHoursToMinutesDelimiterActiveState(false);
		}

		internal class Factory : PlaceholderFactory<ITimerView, ITimerContext, TimerPresenter>
		{
		}
	}
}