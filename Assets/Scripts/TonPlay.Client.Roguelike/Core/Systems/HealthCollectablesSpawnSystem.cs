using System.Linq;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Collectables;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class HealthCollectablesSpawnSystem : IEcsInitSystem, IEcsRunSystem
	{
		private const float SPAWN_RATE = 30f;
		private const float MIN_DISTANCE = 25f;
		private const float MAX_DISTANCE = 40f;

		private readonly ICollectableEntityFactory _collectableEntityFactory;

		private float _spawnTimeLeft = 0f;

		private ICollectableConfig[] _collectableConfigs;

		public HealthCollectablesSpawnSystem(ICollectableEntityFactory collectableEntityFactory)
		{
			_collectableEntityFactory = collectableEntityFactory;
		}

		public void Init(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();

			_collectableConfigs = sharedData.CollectablesConfigProvider.AllCollectables.Where(_ => _.Type == CollectableType.Health).ToArray();
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();

			_spawnTimeLeft -= Time.deltaTime;

			if (_spawnTimeLeft > 0)
			{
				TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
				return;
			}

			_spawnTimeLeft = SPAWN_RATE;

			var randomIndex = Random.Range(0, _collectableConfigs.Length);
			var distance = Random.Range(MIN_DISTANCE, MAX_DISTANCE);
			var position = Random.onUnitSphere.ToVector2XY()*distance + sharedData.PlayerPositionProvider.Position;

			_collectableEntityFactory.Create(world, _collectableConfigs[randomIndex], position);
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}