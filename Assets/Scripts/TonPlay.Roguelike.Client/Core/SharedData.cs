using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collision.Config;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.Core
{
	public class SharedData : ISharedData
	{
		public IPlayerConfigProvider PlayerConfigProvider { get; }

		public IEnemyConfigProvider EnemyConfigProvider { get; }
		
		public IWeaponConfigProvider WeaponConfigProvider { get; }
		
		public ICollisionConfigProvider CollisionConfigProvider { get; }

		public IGameModel GameModel { get; }

		public IPositionProvider PlayerPositionProvider { get; private set; }
		
		public string PlayerWeaponId { get; private set; }

		public SharedData(
			IPlayerConfigProvider playerConfigProvider,
			IEnemyConfigProvider enemyConfigProvider,
			IWeaponConfigProvider weaponConfigProvider,
			IGameModelProvider gameModelProvider, 
			ICollisionConfigProvider collisionConfigProvider)
		{
			PlayerConfigProvider = playerConfigProvider;
			EnemyConfigProvider = enemyConfigProvider;
			WeaponConfigProvider = weaponConfigProvider;
			CollisionConfigProvider = collisionConfigProvider;
			GameModel = gameModelProvider.Get();
		}

		public void SetPlayerPositionProvider(IPositionProvider positionProvider)
		{
			PlayerPositionProvider = positionProvider;
		}

		public void SetPlayerWeapon(string weaponId)
		{
			PlayerWeaponId = weaponId;
		}

		public class Factory : PlaceholderFactory<SharedData>
		{
		}
	}
}