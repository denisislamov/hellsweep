using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Components.Enemies
{
	public struct RotateMovementDirectionToTargetWhenDistanceExceededComponent
	{
		public float Distance;
		public Vector2 CachedDirection;
	}
}