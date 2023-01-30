using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public abstract class ScreenFactory<TContext, TScreen> : IScreenFactory<TContext, TScreen>
		where TScreen : Screen
		where TContext : IScreenContext
	{
		private readonly DiContainer _container;
		private readonly Screen _screenPrefab;

		protected ScreenFactory(DiContainer container, Screen screenPrefab)
		{
			_container = container;
			_screenPrefab = screenPrefab;
		}

		public TScreen Create(TContext context, Transform parent, IScreenLayer screenLayer)
		{
			return _container.InstantiatePrefabForComponent<TScreen>(_screenPrefab, parent, new object[] {context, screenLayer});
		}
	}
}