using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class GameOverSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<PlayerComponent>().Inc<DeadComponent>().End();
			
			foreach (var entity in filter)
			{
				Time.timeScale = 0;
			}
		}
	}
}