using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Interfaces
{
	public interface ISharedData
	{
		IPlayerSpawnConfigProvider PlayerSpawnConfigProvider { get; }

		IEnemySpawnConfigProvider EnemySpawnConfigProvider { get; }
		
		IGameModel GameModel { get; }

		IPositionProvider PlayerPositionProvider { get; }

		IReadOnlyDictionary<Collider2D, EcsEntity> ColliderToEntityMap { get; }
	}
}