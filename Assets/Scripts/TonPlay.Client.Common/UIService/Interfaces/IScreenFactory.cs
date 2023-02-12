using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IScreenFactory<in TContext, out TScreen> : IFactory<TContext, Transform, IScreenLayer, TScreen>
		where TScreen : IScreen
		where TContext : IScreenContext 
	{}
}