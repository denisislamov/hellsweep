using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Systems.Enemies.BossFireCaster;
using TonPlay.Client.Roguelike.Core.Systems.Enemies.BossJadeWarrior;
using TonPlay.Client.Roguelike.Core.Systems.Enemies.BossUnicorn;
using TonPlay.Client.Roguelike.Core.Systems.Enemies.ShadowCaster;
using TonPlay.Client.Roguelike.Core.Systems.Enemies.TerracottaHorseman;
using TonPlay.Client.Roguelike.Core.Systems.Locations.Sands;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations.Sands
{
	internal class SandsGameControllerExtension : GameControllerExtension
	{
		[SerializeField]
		private SandStormEffect _sandsParticles;
		
		[SerializeField]
		private RavenView _ravenView;
		
		private EcsSystems _updateSystems;

		public override void OnInit()
		{
			_updateSystems = new EcsSystems(GameController.MainWorld, GameController.SharedData)
			   .AddWorld(GameController.EffectsWorld, RoguelikeConstants.Core.EFFECTS_WORLD_NAME);
			
			_updateSystems.Add(new SandStormSystem(_sandsParticles))
						  .Add(new SandStormCameraShakeSystem())
						  .Add(new RavenSystem(_ravenView))
						  .Add(new BossUnicornFollowStateSystem())
						  .Add(new BossUnicornTankStateSystem())
						  .Add(new BossFireCasterFollowStateSystem())
						  .Add(new BossFireCasterShootStateSystem())
						  .Add(new ShadowCasterFollowStateSystem())
						  .Add(new ShadowCasterShootStateSystem())
						  .Add(new TerracottaHorsemanAnimatorSystem())
						  .Add(new BossJadeWarriorFollowStateSystem())
						  .Add(new BossJadeWarriorTankStateSystem());
			
			_updateSystems.Init();
		}
		
		public override void OnUpdate()
		{
			_updateSystems.Run();
		}
		
		public override void OnFixedUpdate()
		{
		}
	}
}