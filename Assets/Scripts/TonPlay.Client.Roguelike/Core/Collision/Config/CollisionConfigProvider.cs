using System;
using System.Linq;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Collision.Config
{
	[CreateAssetMenu(fileName = nameof(CollisionConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(CollisionConfigProvider))]
	public class CollisionConfigProvider : ScriptableObject, ICollisionConfigProvider
	{
		[SerializeField]
		private CollisionConfig[] _configs;

		public ICollisionConfig Get(int layer) => _configs.FirstOrDefault(_ => _.Layer == layer);
		
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