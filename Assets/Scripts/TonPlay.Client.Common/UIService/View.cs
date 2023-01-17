using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public abstract class View : MonoBehaviour, IView
	{
		public virtual void Show()
		{
			gameObject.SetActive(true);
		}
		
		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}