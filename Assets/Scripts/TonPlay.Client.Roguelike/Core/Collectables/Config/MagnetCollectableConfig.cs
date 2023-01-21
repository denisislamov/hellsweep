using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Collectables.Config;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collectables.Config
{
	[CreateAssetMenu(fileName = nameof(MagnetCollectableConfig), menuName = AssetMenuConstants.COLLECTABLES_CONFIGS + nameof(MagnetCollectableConfig))]
	public class MagnetCollectableConfig : CollectableConfig, IMagnetCollectableConfig
	{
		[SerializeField]
		private float _magnetRadius;
		
		[SerializeField]
		private float _timeToStart;
		
		[SerializeField]
		private float _activeTime;
		public override CollectableType Type => CollectableType.Magnet;
		
		public float MagnetRadius => _magnetRadius;
		public float TimeToStart => _timeToStart;
		public float ActiveTime => _activeTime;
	}
}