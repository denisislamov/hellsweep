using UnityEngine;

namespace TonPlay.Roguelike.Client.Core
{
	public class EntityIdProvider : MonoBehaviour
	{
		[SerializeField]
		private int _entityId;

		public int EntityId => _entityId;
		
		public void SetEntityId(int entityId)
		{
			_entityId = entityId;
		}
	}
}