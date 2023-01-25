using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using UniRx;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Extensions
{
	public static class EcsEntityAddComponentExtensions
	{
		public static ref ViewPoolObjectComponent AddPoolObjectComponent(this EcsEntity entity, IViewPoolObject viewPoolObject)
		{
			ref var viewPoolObjectComponent = ref entity.Add<ViewPoolObjectComponent>();
			viewPoolObjectComponent.ViewPoolObject = viewPoolObject;

			return ref viewPoolObjectComponent;
		}
		
		public static ref CollectableComponent AddCollectableComponent(this EcsEntity entity, ICollectableConfig config)
		{
			ref var collectable = ref entity.Add<CollectableComponent>();
			collectable.Type = config.Type;
			collectable.Value = config.Value;

			return ref collectable;
		}

		public static ref LayerComponent AddLayerComponent(this EcsEntity entity, int layer)
		{
			ref var layerComponent = ref entity.Add<LayerComponent>();
			layerComponent.Layer = layer;
			return ref layerComponent;
		}

		public static ref PositionComponent AddPositionComponent(this EcsEntity entity, Vector2 position)
		{
			ref var positionComponent = ref entity.Add<PositionComponent>();
			positionComponent.Position = position;
			return ref positionComponent;
		}
		
		public static ref TransformComponent AddTransformComponent(this EcsEntity entity, Transform transform)
		{
			ref var transformComponent = ref entity.Add<TransformComponent>();
			transformComponent.Transform = transform;
			return ref transformComponent;
		}
		
		public static ref ViewProviderComponent AddViewProviderComponent(this EcsEntity entity, GameObject gameObject)
		{
			ref var viewProviderComponent = ref entity.Add<ViewProviderComponent>();
			viewProviderComponent.View = gameObject;
			return ref viewProviderComponent;
		}
		
		public static ref StickToLocationBlockComponent AddStickToLocationBlockComponent(this EcsEntity entity)
		{
			ref var stickToLocationBlockComponent = ref entity.Add<StickToLocationBlockComponent>();
			return ref stickToLocationBlockComponent;
		}
		
		public static ref MagnetCollectableComponent AddMagnetCollectableComponent(this EcsEntity entity, IMagnetCollectableConfig config)
		{
			ref var component = ref entity.Add<MagnetCollectableComponent>();
			component.Config = config;
			return ref component;
		}
		
		public static ref BombCollectableComponent AddBombCollectableComponent(this EcsEntity entity, IBombCollectableConfig config)
		{
			ref var component = ref entity.Add<BombCollectableComponent>();
			component.Config = config;
			return ref component;
		}
		
		public static ref SkillsComponent AddSkillsComponent(this EcsEntity entity)
		{
			ref var skillsComponent = ref entity.Add<SkillsComponent>();
			skillsComponent.Levels = new Dictionary<SkillName, int>();
			return ref skillsComponent;
		}
		
		public static ref RotationComponent AddRotationComponent(this EcsEntity entity, Vector2 direction)
		{
			ref var rotationComponent = ref entity.Add<RotationComponent>();
			rotationComponent.Direction = direction;
			return ref rotationComponent;
		}
		
		public static ref SpeedComponent AddSpeedComponent(this EcsEntity entity, IMovementConfig movementConfig)
		{
			ref var speedComponent = ref entity.Add<SpeedComponent>();
			speedComponent.Speed = movementConfig.StartSpeed;
			return ref speedComponent;
		}
		
		public static ref MovementComponent AddMovementComponent(this EcsEntity entity)
		{
			return ref entity.Add<MovementComponent>();
		}
		
		public static ref RigidbodyComponent AddRigidbodyComponent(this EcsEntity entity, Rigidbody2D rigidbody)
		{
			ref var rigidbodyComponent = ref entity.Add<RigidbodyComponent>();
			rigidbodyComponent.Rigidbody = rigidbody;
			return ref rigidbodyComponent;
		}
		
		public static ref  HealthComponent AddHealthComponent(this EcsEntity entity, float startHealth, float maxHealth)
		{
			ref var healthComponent = ref entity.Add<HealthComponent>();
			healthComponent.CurrentHealth = startHealth;
			healthComponent.MaxHealth = maxHealth;
			return ref healthComponent;
		}
		
		public static ref ProfileExperienceComponent AddProfileExperienceComponent(this EcsEntity entity)
		{
			return ref entity.Add<ProfileExperienceComponent>();
		}
		
		public static ref GoldComponent AddGoldComponent(this EcsEntity entity)
		{
			return ref entity.Add<GoldComponent>();
		}
		
		public static ref ExperienceComponent AddExperienceComponent(this EcsEntity entity, float value, float maxValue, int level)
		{
			ref var exp = ref entity.Add<ExperienceComponent>();
			exp.Value = value;
			exp.MaxValue = maxValue;
			exp.Level = level;
			return ref exp;
		}
		
		public static ref MagnetizableComponent AddMagnetizableComponent(this EcsEntity entity)
		{
			return ref entity.Add<MagnetizableComponent>();
		}
		
		public static ref ExplosionComponent AddExplosionComponent(this EcsEntity entity, float damage, ICollisionAreaConfig collisionAreaConfig, int layerMask)
		{
			ref var explosion = ref entity.Add<ExplosionComponent>();
			explosion.CollisionAreaConfig = collisionAreaConfig;
			explosion.Damage = damage;
			explosion.LayerMask = layerMask;
			return ref explosion;
		}
	}
}