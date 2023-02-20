using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Actions.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	public abstract class ScriptableAction : ScriptableObject, IAction
	{
		public abstract void Execute(int callerEntityIdx, ISharedData sharedData);
	}
}