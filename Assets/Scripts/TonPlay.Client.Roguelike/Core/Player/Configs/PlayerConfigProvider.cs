using System;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Player.Views;
using TonPlay.Roguelike.Client.Core.Movement;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Player.Configs
{
	[CreateAssetMenu(fileName = nameof(PlayerConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(PlayerConfigProvider))]
	public class PlayerConfigProvider : ScriptableObject, IPlayerConfigProvider
	{
		[SerializeField]
		private PlayerConfig[] _configs;

		[SerializeField]
		private string _defaultConfigId;

		public IPlayerConfig Get(string id = default)
		{
			if (string.IsNullOrEmpty(id))
			{
				id = _defaultConfigId;
			}

			return _configs.FirstOrDefault(config => config.Id == id);
		}

		[Serializable]
		private class PlayerConfig : IPlayerConfig
		{
			[SerializeField]
			private string _id;

			[SerializeField]
			private PlayerView _prefab;

			[SerializeField]
			private int _health;

			[SerializeField]
			private CollisionAreaConfig _collisionAreaConfig;
			
			[SerializeField]
			private CollisionAreaConfig _collectablesCollisionAreaConfig;

			[SerializeField]
			private LayerMask _collisionAreaMask;

			[SerializeField]
			private MovementConfig _movementConfig;

			public string Id => _id;

			public PlayerView Prefab => _prefab;

			public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
			
			public ICollisionAreaConfig CollectablesCollisionAreaConfig => _collectablesCollisionAreaConfig;

			public int StartHealth => _health;

			public IMovementConfig MovementConfig => _movementConfig;

			public int CollisionAreaMask => _collisionAreaMask.value;
		}
	}
}