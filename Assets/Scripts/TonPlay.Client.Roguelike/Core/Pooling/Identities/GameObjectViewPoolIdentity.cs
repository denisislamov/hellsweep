using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Pooling.Identities
{
	public class GameObjectViewPoolIdentity : IViewPoolIdentity
	{
		public string Id { get; }

		public GameObjectViewPoolIdentity(GameObject view)
		{
			Id = string.Format("GameObject.{0}", view.name);
		}
	}
}