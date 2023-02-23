using System;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Drops
{
	[Serializable]
	internal class WeightedCollectableIdDropConfig
	{
		[SerializeField]
		private string _collectableId;

		[SerializeField]
		[Range(0, 1f)]
		private float _weight;

		public string CollectableId => _collectableId;

		public float Weight => _weight;
	}
}