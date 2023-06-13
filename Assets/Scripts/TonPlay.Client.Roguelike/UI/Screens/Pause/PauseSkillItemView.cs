using TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause
{
	public class PauseSkillItemView : View, IPauseSkillItemView
	{
		[SerializeField]
		private Image _icon;

		[FormerlySerializedAs("_activeLevelsImages")] [SerializeField]
		private Image[] _backgroundLevelsImages;

		[FormerlySerializedAs("_backgroundLevelsImages")] [SerializeField]
		private Image[] _activeLevelsImages;
		
		public void SetIcon(Sprite icon)
		{
			_icon.sprite = icon;
		}
		
		public void SetCurrentLevel(int level)
		{
			for (var i = 0; i < _activeLevelsImages.Length; i++)
			{
				_activeLevelsImages[i].gameObject.SetActive(level - i - 1 >= 0);
				_activeLevelsImages[i].color = Color.white;
			}
		}
		
		public void SetMaxLevel(int level)
		{
			for (var i = 0; i < _backgroundLevelsImages.Length; i++)
			{
				_backgroundLevelsImages[i].gameObject.SetActive(level - i - 1 >= 0);
			}
		}
		
		public void SetColor(Color color)
		{
			_icon.color = color;
		}
	}
}