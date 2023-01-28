using System;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Interfaces
{
	public interface IClickableView : IView
	{
		IObservable<Unit> OnClick { get; }
	}
}