using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components.Enemies.TerracottaHorseman;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(AssignTerracottaHorsemanComponent),
		menuName = AssetMenuConstants.ACTIONS + "TerracottaHorseman/" + nameof(AssignTerracottaHorsemanComponent))]
	public class AssignTerracottaHorsemanComponent : ScriptableAction
	{
		[SerializeField] 
		private float _attackAnimDistance;
		
		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var entity = new EcsEntity(sharedData.MainWorld, callerEntityIdx);

			ref var horseman = ref entity.Add<TerracottaHorsemanComponent>();
			horseman.AttackAnimDistance = _attackAnimDistance;
		}
	}
}