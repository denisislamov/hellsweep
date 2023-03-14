using UnityEngine;
using UnityEngine.Playables;

namespace TonPlay.Client.Roguelike.Core.Weapons.Views
{
	public class ProjectileView : MonoBehaviour
	{
		[SerializeField]
		private PlayableDirector _playableDirector;
		
		public PlayableDirector PlayableDirector => _playableDirector;
	}
}