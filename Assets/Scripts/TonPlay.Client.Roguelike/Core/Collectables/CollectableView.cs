using TonPlay.Roguelike.Client.Core.Collectables.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Collectables
{
	public class CollectableView : MonoBehaviour, ICollectableView
	{
		public Transform Transform => transform;
	}
}