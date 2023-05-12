using System;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

namespace TonPlay.Client.Common.Utilities
{
	public class ObservablePlayableDirectorTrigger : MonoBehaviour
	{
		private Subject<Unit> _onFinishedPlayingTriggerAsObservable = new Subject<Unit>();
		
		private PlayableDirector _playableDirector;

		private void Awake()
		{
			_playableDirector = GetComponent<PlayableDirector>();
		}

		public IObservable<Unit> OnFinishedPlayingTriggerAsObservable()
		{
			_playableDirector.stopped += PlayableDirectorOnStopped;
			return _onFinishedPlayingTriggerAsObservable;
		}
		
		private void PlayableDirectorOnStopped(PlayableDirector component)
		{
			_onFinishedPlayingTriggerAsObservable.OnNext(Unit.Default);
		}
	}
}