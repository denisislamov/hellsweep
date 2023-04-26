using TMPro;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade
{
	public class InventoryItemGradeDescriptionView : View, IInventoryItemGradeDescriptionView
	{
		[SerializeField] 
		private TMP_Text _text;

		[SerializeField]
		private Image _backgroundImage;

		[SerializeField]
		private GameObject _lockGameObject;

		[SerializeField]
		private GameObject _checkGameObject;

		[SerializeField]
		private RectTransform _textLayoutGroup;
		
		[SerializeField]
		private RectTransform _mainLayoutGroup;

		[SerializeField]
		private LayoutGroup[] _layoutGroups;
		
		public void SetText(string text)
		{
			_text.SetText(text);
		}
		
		public void SetUnlockedState(bool state)
		{
			_lockGameObject.SetActive(!state);
			_checkGameObject.SetActive(state);
		}
		
		public void SetIconBackgroundColor(Color color)
		{
			_backgroundImage.color = color;
		}

		public void UpdateTextLayout()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(_textLayoutGroup);
		}
		
		public void SetGradeLayoutActiveState(bool state)
		{
			for (var i = 0; i < _layoutGroups.Length; i++)
			{
				_layoutGroups[i].enabled = state;
			}
		}

		public void UpdateMainLayout()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(_mainLayoutGroup);
		}
	}
}