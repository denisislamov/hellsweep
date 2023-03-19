using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.LocationSlider
{
	internal class LocationSliderPresenter : Presenter<ILocationSliderView, ILocationSliderContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ILocationConfigProvider _locationConfigProvider;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private int _currentLocationConfigIndex;
		private IButtonPresenter _leftButtonPresenter;
		private IButtonPresenter _rightButtonPresenter;

		public LocationSliderPresenter(
			ILocationSliderView view,
			ILocationSliderContext context,
			IButtonPresenterFactory buttonPresenterFactory,
			ILocationConfigProvider locationConfigProvider,
			IMetaGameModelProvider metaGameModelProvider)
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			_locationConfigProvider = locationConfigProvider;
			_metaGameModelProvider = metaGameModelProvider;

			AddNestedButtonsPresenter();
			SelectLocation(_currentLocationConfigIndex);
			RefreshView();
		}

		public override void Show()
		{
			base.Show();

			RefreshView();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();

			base.Dispose();
		}

		private void AddNestedButtonsPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.LeftButton,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => OnSliderButtonClickHandler(-1))));

			Presenters.Add(presenter);

			_leftButtonPresenter = presenter;

			presenter = _buttonPresenterFactory.Create(View.RightButton,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => OnSliderButtonClickHandler(+ 1))));

			Presenters.Add(presenter);

			_rightButtonPresenter = presenter;
		}

		private void OnSliderButtonClickHandler(int direction)
		{
			_currentLocationConfigIndex += direction;

			if (_currentLocationConfigIndex < 0)
			{
				_currentLocationConfigIndex = 0;
			}
			else if (_currentLocationConfigIndex >= _locationConfigProvider.Configs.Length)
			{
				_currentLocationConfigIndex = _locationConfigProvider.Configs.Length - 1;
			}

			SelectLocation(_currentLocationConfigIndex);
			RefreshView();
		}

		private void RefreshView()
		{
			if (_currentLocationConfigIndex == 0)
			{
				_leftButtonPresenter.Hide();
			}
			else
			{
				_leftButtonPresenter.Show();
			}

			if (_currentLocationConfigIndex >= _locationConfigProvider.Configs.Length - 1)
			{
				_rightButtonPresenter.Hide();
			}
			else
			{
				_rightButtonPresenter.Show();
			}

			var config = Context.LocationConfigStorageSelector.Current.Value;
			var locationsModel = _metaGameModelProvider.Get().LocationsModel;
			var model = locationsModel.Locations.ContainsKey(config.Id) ? locationsModel.Locations[config.Id] : new LocationModel();
			var longestSurvivedTime = TimeSpan.FromMilliseconds(model.LongestSurvivedMillis.Value);

			View.SetIcon(config.Icon);
			View.SetTitleText(config.Title);
			View.SetSubtitleText($"Longest Survived: {longestSurvivedTime:mm}m {longestSurvivedTime:ss}s");
		}

		private void SelectLocation(int currentLocationConfigIndex)
		{
			Context.LocationConfigStorageSelector.Select(_locationConfigProvider.Configs[currentLocationConfigIndex]);
		}

		internal class Factory : PlaceholderFactory<ILocationSliderView, ILocationSliderContext, LocationSliderPresenter>
		{
		}
	}
}