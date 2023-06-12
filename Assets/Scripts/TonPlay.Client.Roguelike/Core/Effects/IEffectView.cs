using UnityEngine.Playables;

namespace TonPlay.Client.Roguelike.Core.Effects
{
	public interface IEffectView
	{
		PlayableDirector PlayableDirector { get; }
	}
}