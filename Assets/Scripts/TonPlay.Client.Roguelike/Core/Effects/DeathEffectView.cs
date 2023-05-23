using UnityEngine;
using UnityEngine.Playables;

namespace TonPlay.Client.Roguelike.Core.Effects
{
	public class DeathEffectView : MonoBehaviour
	{
		[SerializeField]
		private PlayableDirector _playableDirector;
		
		public PlayableDirector PlayableDirector => _playableDirector;
	}
}