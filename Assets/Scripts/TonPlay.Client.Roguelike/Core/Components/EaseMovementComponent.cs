using DG.Tweening;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct EaseMovementComponent
	{
		public Ease Ease;
		public Vector2 FromPosition;
		public Vector2 ToPosition;
		public float ActiveTime;
		public float Duration;
	}
}