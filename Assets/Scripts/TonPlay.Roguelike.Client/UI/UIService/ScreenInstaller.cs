using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class ScreenInstaller : ScriptableObjectInstaller
	{
		[SerializeField]
		private Screen _screen;

		public Screen ScreenPrefab => _screen;
	}
}