using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(DespawnArenaAction),
		menuName = AssetMenuConstants.ACTIONS + nameof(DespawnArenaAction))]
	public class DespawnArenaAction : ScriptableAction
	{
		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var world = sharedData.MainWorld;
			var filter = world.Filter<Arena>().Exc<DestroyComponent>().End();
			var destroyPool = world.GetPool<DestroyComponent>();

			foreach (var entityId in filter)
			{
				sharedData.ArenasKdTreeStorage.RemoveEntity(entityId);

				destroyPool.AddOrGet(entityId);
			}
		}
	}
}