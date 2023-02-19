using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class BlockTimerApplyDamageSystem : IEcsRunSystem
	{
		private readonly Dictionary<string, Stack<int>> _stacks = new Dictionary<string, Stack<int>>();
		private readonly string[] _damageSources;

		public BlockTimerApplyDamageSystem()
		{
			_damageSources = Enum.GetNames(typeof(DamageSource));

			foreach (var damageSource in _damageSources)
			{
				_stacks[damageSource] = new Stack<int>();
			}
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<BlockApplyDamageTimerComponent>()
						.End();

			var blockPool = world.GetPool<BlockApplyDamageTimerComponent>();

			foreach (var entityId in filter)
			{
				ref var block = ref blockPool.Get(entityId);
				foreach (var damageSourceToDict in block.Blocked)
				{
					
				}

				for (var i = 0; i < _damageSources.Length; i++)
				{
					var damageSource = _damageSources[i];

					if (!block.Blocked.ContainsKey(damageSource))
					{
						continue;
					}

					var entitiesToTimerDict = block.Blocked[damageSource];
					
					foreach (var entityIdToTimer in entitiesToTimerDict)
					{
						entitiesToTimerDict[entityIdToTimer.Key]
						   .SetValueAndForceNotify(entitiesToTimerDict[entityIdToTimer.Key].Value - Time.deltaTime);

						if (entitiesToTimerDict[entityIdToTimer.Key].Value < 0)
						{
							_stacks[damageSource].Push(entityIdToTimer.Key);
						}
					}

					while (_stacks[damageSource].Count > 0)
					{
						var removeEntityId = _stacks[damageSource].Pop();
						block.Blocked[damageSource].Remove(removeEntityId);
					}
					
					_stacks[damageSource].Clear();
				}
			}
		}
	}
}