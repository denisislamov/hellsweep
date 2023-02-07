using System.Linq;
using TonPlay.Roguelike.Client.Core.Movement;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(ProjectileConfig), menuName = AssetMenuConstants.PROJECTILE_CONFIGS + nameof(ProjectileConfig))]
	internal class ProjectileConfig : ScriptableObject, IProjectileConfig
	{
		[SerializeField]
		private ProjectileView _prefab;

		[SerializeField]
		private MovementConfig _movementConfig;
		
		[SerializeField]
		private ProjectileConfigProperty[] _properties;

		public ProjectileView PrefabView => _prefab;
		public IMovementConfig MovementConfig => _movementConfig;
		
		public bool TryGetProperty<T>(out T property) where T : IProjectileConfigProperty
		{
			if (_properties is null)
			{
				property = default(T);
				return false;
			}
			
			var result = _properties
						.Select(_ => (IProjectileConfigProperty) _)
						.FirstOrDefault(_ => _ is T);

			if (result is null)
			{
				property = default(T);
				return false;
			}

			property = (T) result;
			return true;
		}
	}
}