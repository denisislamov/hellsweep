using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Player.Views.Interfaces
{
	public interface IPlayerView : IPositionProvider, IHasWeaponSpawnRoot
	{
		Rigidbody2D Rigidbody2D { get; }

		Animator Animator { get; }
		
		Animator BloodAnimator { get; }
		
		SpriteRenderer[] SpriteRenderers { get; }
		
		float AttackAnimationDuration { get; }
	}
}