using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Views.Interfaces
{
	public interface IEnemyView
	{
		Collider2D Collider2D { get; }

		Rigidbody2D Rigidbody2D { get; }
		
		Animator Animator { get; }
		
		SpriteRenderer[] SpriteRenderers { get; }
	}
}