using System;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

namespace TonPlay.Client.Common.Utilities
{
	public class ObservablePlayableDirectorTrigger : MonoBehaviour
	{
		private Subject<Unit> _onFinishedPlayingTriggerAsObservable = new Subject<Unit>();
		
		public IObservable<Unit> OnFinishedPlayingTriggerAsObservable()
		{
			GetComponent<PlayableDirector>().stopped += PlayableDirectorOnStopped;
			return _onFinishedPlayingTriggerAsObservable;
		}
		
		private void PlayableDirectorOnStopped(PlayableDirector component)
		{
			_onFinishedPlayingTriggerAsObservable.OnNext(Unit.Default);
		}
	}
}