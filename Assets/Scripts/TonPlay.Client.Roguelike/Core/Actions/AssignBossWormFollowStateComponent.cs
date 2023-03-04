using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(AssignBossWormFollowStateComponent),
		menuName = AssetMenuConstants.ACTIONS + "BossWorm/" + nameof(AssignBossWormFollowStateComponent))]
	public class AssignBossWormFollowStateComponent : ScriptableAction
	{
		[SerializeField]
		private float _followSpeed;
		
		[SerializeField]
		private float _followStateDuration;
		
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
		private float _shootDelay;

		[SerializeField]
		private int _projectileQuantity;

		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var entity = new EcsEntity(sharedData.World, callerEntityIdx);

			ref var bossWormComponent = ref entity.Add<BossWormComponent>();
			bossWormComponent.ShootDelay = _shootDelay;
			bossWormComponent.FollowSpeed = _followSpeed;
			bossWormComponent.FollowStateDuration = _followStateDuration;
			bossWormComponent.TankSpeed = _tankRunningSpeed;
			bossWormComponent.TankRunningDuration = _tankRunningDuration;
			bossWormComponent.TankPreparingDuration = _tankPreparingDuration;
			bossWormComponent.TankStoppingDuration = _tankStoppingDuration;
			bossWormComponent.ShootStateDuration = _shootStateDuration;
			bossWormComponent.ProjectileConfig = _projectileConfig;
			bossWormComponent.ProjectileQuantity = _projectileQuantity;
			
			entity.Add<BossWormFollowStateComponent>();
			
			entity.AddOrGet<MovementComponent>();
			entity.AddOrGet<RotationComponent>();
			entity.AddOrGet<SpeedComponent>();
		}
	}
}