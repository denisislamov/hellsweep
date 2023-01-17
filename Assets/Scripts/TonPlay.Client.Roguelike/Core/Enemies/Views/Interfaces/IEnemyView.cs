using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Enemies.Views.Interfaces
{
	public interface IEnemyView
	{
		Collider2D Collider2D { get; }	
		
		Rigidbody2D Rigidbody2D { get; }	
	}
}