using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Rewards
{
	[CreateAssetMenu(fileName = nameof(RewardPresentationProvider), menuName = AssetMenuConstants.UI_CONFIGS + nameof(RewardPresentationProvider))]
	public class RewardPresentationProvider : ScriptableObject, IRewardPresentationProvider
	{
		[SerializeField]
		private RewardPresentation[] _presentations;

		[SerializeField]
		private RewardPresentation _default;

		private Dictionary<string, IRewardPresentation> _map;

		public IReadOnlyDictionary<string, IRewardPresentation> Map => 
			_map ??= _presentations.ToDictionary(_ => _.Id, _ => (IRewardPresentation)_);

		public IRewardPresentation Get(string id) => Map.ContainsKey(id) 
			? Map[id] 
			: _default;

		[Serializable]
		private class RewardPresentation : IRewardPresentation
		{
			[SerializeField]
			private string _id;
			
			[SerializeField]
			private Sprite _icon;
			
			public string Id => _id;
			
			public Sprite Icon => _icon;
		}
	}
}