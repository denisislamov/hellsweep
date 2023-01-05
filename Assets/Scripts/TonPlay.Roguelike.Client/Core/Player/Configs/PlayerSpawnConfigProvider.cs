using System;
using System.Linq;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;
using TonPlay.Roguelike.Client.Core.Player.Views.Intefacves;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Player.Configs
{
	[CreateAssetMenu(fileName = nameof(PlayerSpawnConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(PlayerSpawnConfigProvider))]
	public class PlayerSpawnConfigProvider : ScriptableObject, IPlayerSpawnConfigProvider
	{
		[SerializeField]
		private PlayerSpawnConfig[] _configs;
		
		[SerializeField]
		private string _defaultConfigId;
		
		public IPlayerSpawnConfig Get(string id = default)
		{
			if (string.IsNullOrEmpty(id))
			{
				id = _defaultConfigId;
			}
			
			return _configs.FirstOrDefault(config => config.Id == id);
		}
		
		[Serializable]
		private class PlayerSpawnConfig : IPlayerSpawnConfig
		{
			[SerializeField]
			private string _id;
		
			[SerializeField]
			private PlayerView _prefab;

			[SerializeField]
			private int _health;

			public string Id => _id;
		
			public PlayerView Prefab => _prefab;
			
			public int StartHealth => _health;
		}
	}
}