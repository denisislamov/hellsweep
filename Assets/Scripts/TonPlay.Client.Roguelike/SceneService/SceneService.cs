using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine.SceneManagement;
using Zenject;

namespace TonPlay.Client.Roguelike.SceneService
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
		
		public UniTask LoadSingleSceneWithZenjectByNameAsync(SceneName name, Action<DiContainer> extraBindings = null)
		{
			return _zenjectSceneLoader.LoadSceneAsync(name.ToString(), LoadSceneMode.Single, extraBindings).ToUniTask();
		}
		
		public UniTask UnloadAdditiveSceneByNameAsync(SceneName sceneName)
		{
			return SceneManager.UnloadSceneAsync(sceneName.ToString()).ToUniTask();
		}
		
		public UniTask LoadAdditiveSceneWithZenjectByNameAsync(SceneName name, Action<DiContainer> extraBindings = null)
		{
			return _zenjectSceneLoader.LoadSceneAsync(name.ToString(), LoadSceneMode.Additive, extraBindings).ToUniTask();
		}
	}
}