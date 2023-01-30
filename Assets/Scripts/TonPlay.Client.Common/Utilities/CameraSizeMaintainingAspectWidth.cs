using UnityEngine;

namespace TonPlay.Client.Common.Utilities
{
	[RequireComponent(typeof(Camera))]
	public class CameraSizeMaintainingAspectWidth : MonoBehaviour
	{
		[SerializeField]
		private float _defaultWidth;
		
		private void Start()
		{
			var camera = GetComponent<Camera>();
			camera.orthographicSize = _defaultWidth / camera.aspect;
		}
	}
}