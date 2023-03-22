using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossButcher;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(AssignBossButcherComponentAndFollowState),
		menuName = AssetMenuConstants.ACTIONS + "BossButcher/" + nameof(AssignBossButcherComponentAndFollowState))]
	public class AssignBossButcherComponentAndFollowState : ScriptableAction
	{
		[SerializeField]
		private float _followSpeed;
		
		[SerializeField]
		private float _followStateDuration;
		
		[SerializeField]
		private DamageProvider _followDamageProvider;
		
		[SerializeField]
		private float _tankRunningSpeed;
		
		[SerializeField]
		private float _tankRunningDuration;
		
		[SerializeField]
		private float _tankPreparingDuration;
		
		[SerializeField]
		private float _tankStoppingDuration;
		
		[SerializeField]
		private DamageProvider _tankDamageProvider;

		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var entity = new EcsEntity(sharedData.World, callerEntityIdx);

			ref var bossWormComponent = ref entity.Add<BossButcherComponent>();
			bossWormComponent.FollowSpeed = _followSpeed;
			bossWormComponent.FollowStateDuration = _followStateDuration;
			bossWormComponent.FollowDamageProvider = _followDamageProvider;
			
			bossWormComponent.TankSpeed = _tankRunningSpeed;
			bossWormComponent.TankRunningDuration = _tankRunningDuration;
			bossWormComponent.TankPreparingDuration = _tankPreparingDuration;
			bossWormComponent.TankStoppingDuration = _tankStoppingDuration;
			bossWormComponent.TankDamageProvider = _tankDamageProvider;
			
			entity.Add<BossButcherFollowStateComponent>();
			
			entity.AddOrGet<MovementComponent>();
			entity.AddOrGet<RotationComponent>();
			entity.AddSpeedComponent(_followSpeed);
		}
	}
}