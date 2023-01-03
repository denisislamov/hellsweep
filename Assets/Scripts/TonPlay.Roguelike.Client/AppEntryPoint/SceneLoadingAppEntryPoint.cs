using System;
using Cysharp.Threading.Tasks;
using TonPlay.Roguelike.Client.AppEntryPoint.Interfaces;
using TonPlay.Roguelike.Client.SceneService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;

namespace TonPlay.Roguelike.Client.AppEntryPoint
{
	public class SceneLoadingAppEntryPoint : IAppEntryPoint
	{
		private readonly ISceneService _sceneService;
		private readonly string _sceneName;

		public SceneLoadingAppEntryPoint(ISceneService sceneService, string sceneName) 
		{
			_sceneService = sceneService;
			_sceneName = sceneName;
		}

		public virtual async UniTask ProcessEntrance()
		{
			await LoadScene();
		}

		public virtual async UniTask ProcessReboot() => await LoadScene();

		private async UniTask LoadScene()
		{
			if (Enum.TryParse(_sceneName, out SceneName sceneName))
			{
				await _sceneService.LoadSingleSceneByNameAsync(sceneName);
			}
			else
			{
				throw new InvalidOperationException($"{_sceneName} can't be loaded. Check if scene name is defined in SceneName scope.");
			}
		}
	}
}