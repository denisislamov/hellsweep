using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Pooling.Creators;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Pooling
{
	[CreateAssetMenu(fileName = nameof(LevelPoolObjectCreatorList), menuName = AssetMenuConstants.POOLING_CONFIGS + nameof(LevelPoolObjectCreatorList))]
	public class LevelPoolObjectCreatorList : ScriptableObject, ILevelPoolObjectCreatorList
	{
		[SerializeField] private PoolObjectCreator[] _creators;
		
		public IReadOnlyList<IPoolObjectCreator> All => _creators;
	}
}