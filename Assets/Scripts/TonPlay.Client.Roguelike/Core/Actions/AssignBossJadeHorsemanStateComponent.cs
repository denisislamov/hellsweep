using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossJadeHorseman;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using BossWormFollowStateComponent = TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm.BossWormFollowStateComponent;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(AssignBossJadeHorsemanStateComponent),
		menuName = AssetMenuConstants.ACTIONS + "BossJadeHorseman/" + nameof(AssignBossJadeHorsemanStateComponent))]
	public class AssignBossJadeHorsemanStateComponent : ScriptableAction
	{
		[SerializeField]
		private int _tankCount;
		
		[SerializeField]
		private float _tankRunningSpeed;
		
		[SerializeField]
		private float _tankRunningDuration;
		
		[SerializeField]
		private float _tankPreparingDuration;
		
		[SerializeField]
		private float _tankStoppingDuration;
		
		[SerializeField]
		private float _shootStateDuration;
		
		[SerializeField]
		private ProjectileConfig _projectileConfig;
		
		[SerializeField]
		private int _shootCount;
		
		[SerializeField]
		private float _shootDelay;

		[SerializeField]
		private int _projectileQuantity;
		
		[SerializeField] 
		private DamageProvider _tankStateDamageProvider;
		
		[SerializeField] 
		private DamageProvider _shootStateDamageProvider;

		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var entity = new EcsEntity(sharedData.MainWorld, callerEntityIdx);

			ref var bossComponent = ref entity.Add<BossJadeHorsemanComponent>();
			bossComponent.ShootDelay = _shootDelay;
			bossComponent.TankCount = _tankCount;
			bossComponent.TankSpeed = _tankRunningSpeed;
			bossComponent.TankRunningDuration = _tankRunningDuration;
			bossComponent.TankPreparingDuration = _tankPreparingDuration;
			bossComponent.TankStoppingDuration = _tankStoppingDuration;
			bossComponent.ShootStateDuration = _shootStateDuration;
			bossComponent.ShootCount = _shootCount;
			bossComponent.ProjectileConfig = _projectileConfig;
			bossComponent.ProjectileQuantity = _projectileQuantity;
			bossComponent.TankStateDamageProvider = _tankStateDamageProvider;
			bossComponent.ShootStateDamageProvider = _shootStateDamageProvider;
			
			entity.Add<BossJadeHorsemanShootStateComponent>();
			
			entity.AddOrGet<MovementComponent>();
			entity.AddOrGet<RotationComponent>();
			entity.AddOrGet<SpeedComponent>();
		}
	}
}