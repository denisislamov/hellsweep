using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class DefaultScreenRoot : MonoBehaviour, IDefaultScreenRoot
	{
		public Transform Root => transform;
	}
}