using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.CollisionProcessors.Interfaces
{
	public interface ICollisionProcessor
	{
		void Process(ref Collider2D collider);
	}
}