using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossFireCaster;
using TonPlay.Client.Roguelike.Core.Components.Enemies.ShadowCasterMiniboss;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(AssignBossFireCasterComponentAndFollowState),
		menuName = AssetMenuConstants.ACTIONS + "BossFireCaster/" + nameof(AssignBossFireCasterComponentAndFollowState))]
	public class AssignBossFireCasterComponentAndFollowState : ScriptableAction
	{
		[SerializeField]
		private float _followSpeed;
		
		[SerializeField]
		private float _followStateDuration;
		
		[SerializeField] 
		private float _initShootDelay;
		
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
			var entity = new EcsEntity(sharedData.MainWorld, callerEntityIdx);

			ref var shadowCasterComponent = ref entity.Add<BossFireCasterComponent>();
			shadowCasterComponent.ShootDelay = _shootDelay;
			shadowCasterComponent.FollowSpeed = _followSpeed;
			shadowCasterComponent.FollowStateDuration = _followStateDuration;
			shadowCasterComponent.ShootStateDuration = _shootStateDuration;
			shadowCasterComponent.InitShootDelay = _initShootDelay;
			shadowCasterComponent.ProjectileConfig = _projectileConfig;
			shadowCasterComponent.ProjectileQuantity = _projectileQuantity;
			
			entity.Add<BossFireCasterFollowStateComponent>();
			
			entity.AddOrGet<MovementComponent>();
			entity.AddOrGet<RotationComponent>();
		}
	}
}