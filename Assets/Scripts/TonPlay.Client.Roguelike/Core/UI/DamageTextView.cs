using TMPro;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.UI
{
	public class DamageTextView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshPro _text;

		[SerializeField]
		private Transform _transform;
		
		public Color Color
		{
			get => _text.color; 
			set => _text.color = value;
		}

		public Vector3 Position
		{
			get => _transform.position; 
			set => _transform.position = value;
		}

		public void SetText(string text)
		{
			_text.SetText(text);
		}
		
		public void Show()
		{
		}
		public void Hide()
		{
		}
		public void SetParent(Transform parent)
		{
		}
	}
}