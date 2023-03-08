using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.SceneService.Interfaces
{
	public interface ISceneService
	{
		UniTask LoadSingleSceneByNameAsync(SceneName sceneName);

		UniTask LoadSingleSceneWithZenjectByNameAsync(SceneName name, Action<DiContainer> extraBindings = null);

		UniTask UnloadAdditiveSceneByNameAsync(SceneName sceneName);

		UniTask LoadAdditiveSceneWithZenjectByNameAsync(SceneName name, Action<DiContainer> extraBindings = null);
	}
}