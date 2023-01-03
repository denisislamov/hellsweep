using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IScreen
	{
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