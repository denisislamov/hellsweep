using System;
using TonPlay.Client.Common.Utilities;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Playables;

namespace TonPlay.Client.Common.Extensions
{
	public static class PlayableDirectorExtensions
	{
		public static IObservable<Unit> OnFinishedPlayingAsObservable(this PlayableDirector component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<Unit>();
			return GetOrAddComponent<ObservablePlayableDirectorTrigger>(component.gameObject).OnFinishedPlayingTriggerAsObservable();
		}
		
		static T GetOrAddComponent<T>(GameObject gameObject)
			where T : Component
		{
			var component = gameObject.GetComponent<T>();
			if (component == null)
			{
				component = gameObject.AddComponent<T>();
			}

			return component;
		}
	}
}