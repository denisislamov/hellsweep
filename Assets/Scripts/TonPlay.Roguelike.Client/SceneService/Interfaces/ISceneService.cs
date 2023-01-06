using System;
using Cysharp.Threading.Tasks;
using TonPlay.Roguelike.Client.Utilities;
using Zenject;

namespace TonPlay.Roguelike.Client.SceneService.Interfaces
{
	public interface ISceneService
	{
		UniTask LoadSingleSceneByNameAsync(SceneName sceneName);

		UniTask LoadSingleSceneWithZenjectByNameAsync(SceneName name, Action<DiContainer> extraBindings = null);

		UniTask UnloadAdditiveSceneByNameAsync(SceneName sceneName);

		UniTask LoadAdditiveSceneWithZenjectByNameAsync(SceneName name, Action<DiContainer> extraBindings = null);
	}
}