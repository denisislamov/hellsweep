using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Drops.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Movement;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs
{
	[CreateAssetMenu(fileName = nameof(EnemyConfigProvider), menuName = AssetMenuConstants.ENEMIES_CONFIGS + nameof(EnemyConfigProvider))]
	public class EnemyConfigProvider : ScriptableObject, IEnemyConfigProvider
	{
		[SerializeField]
		private EnemyConfig[] _configs;
		
		[SerializeField]
		private string _defaultConfigId;
		
		public IEnemyConfig Get(string id = default)
		{
			if (string.IsNullOrEmpty(id))
			{
				id = _defaultConfigId;
			}
			
			return _configs.FirstOrDefault(config => config.Id == id);
		}
	}
}