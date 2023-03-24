using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IDefaultScreenRoot : IScreenLayer
	{
		public Transform Root { get; }
	}
}