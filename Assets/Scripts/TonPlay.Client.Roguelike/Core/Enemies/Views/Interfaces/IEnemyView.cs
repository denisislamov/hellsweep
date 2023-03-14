using UnityEngine;
using UnityEngine.Playables;

namespace TonPlay.Client.Roguelike.Core.Enemies.Views.Interfaces
{
	public interface IEnemyView
	{
		Collider2D Collider2D { get; }

		Rigidbody2D Rigidbody2D { get; }
		
		Animator Animator { get; }
		
		PlayableDirector PlayableDirector { get; }
		
		SpriteRenderer[] SpriteRenderers { get; }
	}
}