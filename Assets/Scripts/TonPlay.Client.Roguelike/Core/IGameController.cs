using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core
{
	internal interface IGameController
	{
		ISharedData SharedData { get; }
		
		EcsWorld MainWorld { get; }
		
		EcsWorld EffectsWorld { get; }
	}
}