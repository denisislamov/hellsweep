using System.Linq;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Movement;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
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
		
		private IViewPoolIdentity _identity;

		public ProjectileView PrefabView => _prefab;
		public IMovementConfig MovementConfig => _movementConfig;
		public IViewPoolIdentity Identity => _identity ??= new ProjectileConfigViewPoolIdentity(this);

		public bool HasProperty<T>() where T : IProjectileConfigProperty
		{
			if (_properties is null)
			{
				return false;
			}

			for (var i = 0; i < _properties.Length; i++)
			{
				var propertyConfig = _properties[i];

				if (propertyConfig is T typedConfig)
				{
					return true;
				}
			}
			
			return false;
		}
		
		public T GetProperty<T>() where T : IProjectileConfigProperty
		{
			if (_properties is null)
			{
				return default(T);
			}

			for (var i = 0; i < _properties.Length; i++)
			{
				var propertyConfig = _properties[i];

				if (propertyConfig is T typedConfig)
				{
					return typedConfig;
				}
			}
			
			return default(T);
		}
	}
}