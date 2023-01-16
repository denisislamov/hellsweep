using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Components
{
	public struct ApplyCollectableComponent
	{
		public int AppliedEntityId;
		public int CollectableEntityId;
		public bool Started;
		public Vector3 StartPosition;
		public float TimeRequired;
		public float TimeLeft;
	}
}