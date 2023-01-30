using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class ScreenInstaller : ScriptableObjectInstaller
	{
		[SerializeField]
		private Screen _screen;

		public Screen ScreenPrefab => _screen;
	}
}