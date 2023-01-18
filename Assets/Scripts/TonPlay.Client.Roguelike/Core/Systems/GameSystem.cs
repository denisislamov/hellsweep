using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class GameSystem : IEcsInitSystem, IEcsRunSystem
	{
		private SharedData _sharedData;
		
		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			
			_sharedData = systems.GetShared<SharedData>();

			var entity = world.NewEntity();
			entity.Add<GameComponent>();
			entity.Add<WavesDataComponent>();
			
			ref var gameTime = ref entity.Add<GameTimeComponent>();
			gameTime.Time = 0;

			UpdateGameModelTime(ref gameTime);
		}
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();

			var filter = world.Filter<GameComponent>().Inc<GameTimeComponent>().End();
			var pool = world.GetPool<GameTimeComponent>();
			
			foreach (var entityId in filter)
			{
				ref var time = ref pool.Get(entityId);
				time.Time += Time.deltaTime;
				
				UpdateGameModelTime(ref time);
			}
		}
		
		private void UpdateGameModelTime(ref GameTimeComponent component)
		{
			var data = _sharedData.GameModel.ToData();
			
			data.GameTime = component.Time;
			
			_sharedData.GameModel.Update(data);
		}
	}
}