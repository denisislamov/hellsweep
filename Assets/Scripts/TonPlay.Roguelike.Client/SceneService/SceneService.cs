using System;
using Cysharp.Threading.Tasks;
using TonPlay.Roguelike.Client.SceneService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine.SceneManagement;
using Zenject;

namespace TonPlay.Roguelike.Client.SceneService
{
	public class SceneService : ISceneService
	{
		private readonly ZenjectSceneLoader _zenjectSceneLoader;

		public SceneService(ZenjectSceneLoader zenjectSceneLoader)
		{
			_zenjectSceneLoader = zenjectSceneLoader;
		}

		public UniTask LoadSingleSceneByNameAsync(SceneName sceneName)
		{
			return SceneManager.LoadSceneAsync(sceneName.ToString()).ToUniTask();
		}
		
		public async UniTask LoadSingleSceneWithZenjectByNameAsync(SceneName name, Action<DiContainer> extraBindings = null)
		{
			await _zenjectSceneLoader.LoadSceneAsync(name.ToString(), LoadSceneMode.Single, extraBindings).ToUniTask();
		}
	}
}