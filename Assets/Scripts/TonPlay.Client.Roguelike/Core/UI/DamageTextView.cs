using TMPro;
using TonPlay.Client.Roguelike.Extensions;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.UI
{
	public class DamageTextView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _text;

		[SerializeField]
		private Transform _transform;
		
		private Color32? _color;

		public Color32 Color
		{
			get => _color ??= _text.color;
			set
			{
				_color = value;

				var newVertexColors = _text.textInfo.meshInfo[0].colors32;

				if (newVertexColors is null)
				{
					return;
				}
				
				for (int i = 0; i < _text.textInfo.characterCount; ++i)
				{
					var meshIndex = _text.textInfo.characterInfo[i].materialReferenceIndex;
					var vertexIndex = _text.textInfo.characterInfo[i].vertexIndex;
					var vertexColors = _text.textInfo.meshInfo[meshIndex].colors32;
					vertexColors[vertexIndex + 0] = value;
					vertexColors[vertexIndex + 1] = value;
					vertexColors[vertexIndex + 2] = value;
					vertexColors[vertexIndex + 3] = value;
				}
				
				_text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			}
		}

		public Vector3 Position
		{
			get => _transform.position;
			set => _transform.position = value;
		}

		public void SetText(int intValue)
		{
			_text.SetText(intValue);
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