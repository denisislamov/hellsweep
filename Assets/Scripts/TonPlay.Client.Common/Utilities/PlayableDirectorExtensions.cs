using UnityEngine.Playables;

namespace TonPlay.Client.Common.Utilities
{
	public static class PlayableDirectorExtensions
	{
		public static void OptimizedPlay(this PlayableDirector playableDirector)
		{
			if (playableDirector.playableGraph.IsValid())
			{
				playableDirector.time = 0f;
			}
			else
			{
				playableDirector.Play();
			}
		}
	}
}