using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations.Sands
{
	public class RavenView : MonoBehaviour
	{
		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private Transform _transform;

		public Animator Animator => _animator;

		public Transform SelfTransform => _transform;
	}
}