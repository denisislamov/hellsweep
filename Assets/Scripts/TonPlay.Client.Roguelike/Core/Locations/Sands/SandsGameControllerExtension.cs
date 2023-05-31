using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Systems.Locations.Sands;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations.Sands
{
	internal class SandsGameControllerExtension : GameControllerExtension
	{
		[SerializeField]
		private SandStormEffect _sandsParticles;
		
		private EcsSystems _updateSystems;
		
		public override void OnInit()
		{
			_updateSystems = new EcsSystems(GameController.MainWorld, GameController.SharedData)
			   .AddWorld(GameController.EffectsWorld, RoguelikeConstants.Core.EFFECTS_WORLD_NAME);
			
			_updateSystems.Add(new SandStormSystem(_sandsParticles))
						  .Add(new SandStormCameraShakeSystem());
			
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