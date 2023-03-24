using System;
using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IScreen : IDisposable
	{
		GameObject GameObject { get; }
		
		IScreenLayer RootLayer { get; }
		
		IScreenStack EmbeddedScreensStack { get; }

		Transform GetEmbeddedTransformRoot();

		void Open();

		void Close();
	}
	
	public interface IScreen<TContext> : IScreen
	{
	}
}