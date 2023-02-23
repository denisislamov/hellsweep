using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Roguelike.Client.Core.Collision.Config;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.Config
{
	[CreateAssetMenu(fileName = nameof(CollisionConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(CollisionConfigProvider))]
	public class CollisionConfigProvider : ScriptableObject, ICollisionConfigProvider
	{
		[SerializeField]
		private CollisionConfig[] _configs;

		private Dictionary<int, CollisionConfig> _map;

		private Dictionary<int, CollisionConfig> Map => _map ??= _configs.ToDictionary(_ => _.Layer, _ => _);

		public ICollisionConfig Get(int layer) => Map.ContainsKey(layer) ? Map[layer] : null;

		[Serializable]
		private class CollisionConfig : ICollisionConfig
		{
			[SerializeField, Layer]
			private int _layer;

			[SerializeField]
			private LayerMask _layerMask;

			public int LayerMask => _layerMask;

			public int Layer => _layer;
		}
	}
}