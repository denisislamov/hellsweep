using System;
using System.Linq;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Player.Configs
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
			
			public string Id => _id;
		
			public PlayerView Prefab => _prefab;

			public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;

			public int StartHealth => _health;
		}
	}
}