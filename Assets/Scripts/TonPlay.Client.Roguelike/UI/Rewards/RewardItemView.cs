using TMPro;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	public class RewardItemView : CollectionItemView, IRewardItemView
	{
		[SerializeField]
		private Image _iconImage;

		[SerializeField]
		private RawImage _backgroundGradient;

		[SerializeField]
		private TextMeshProUGUI _countText;

		[SerializeField]
		private GameObject[] _countObjects;

		public void SetIcon(Sprite icon)
		{
			_iconImage.sprite = icon;
		}
		
		public void SetBackgroundGradientMaterial(Material material)
		{
			if (material == null)
			{
				_backgroundGradient.gameObject.SetActive(false);
				return;
			}

			_backgroundGradient.gameObject.SetActive(true);
			_backgroundGradient.material = material;
		}

		public void SetCountText(string text)
		{
			_countText.SetText(text);
		}
		
		public void SetCountActiveState(bool state)
		{
			for (var i = 0; i < _countObjects.Length; i++)
			{
				var obj = _countObjects[i];
				obj.SetActive(state);
			}
		}
	}
}