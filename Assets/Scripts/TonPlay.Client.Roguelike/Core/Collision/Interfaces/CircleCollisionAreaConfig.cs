using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collision.Interfaces
{
	[CreateAssetMenu(fileName = nameof(CircleCollisionAreaConfig), menuName = AssetMenuConstants.COLLISION_AREAS_CONFIGS + nameof(CircleCollisionAreaConfig))]
	internal class CircleCollisionAreaConfig : CollisionAreaConfig, ICircleCollisionAreaConfig
	{
		[SerializeField]
		private float _radius;

		public float Radius => _radius;

		public void SetRadius(float value)
		{
			_radius = value;
		}
		
		public override CollisionAreaConfig Clone()
		{
			return Instantiate(this);
		}
	}
}