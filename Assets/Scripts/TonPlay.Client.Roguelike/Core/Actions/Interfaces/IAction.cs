using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Actions.Interfaces
{
	public interface IAction
	{
		void Execute(int callerEntityIdx, ISharedData sharedData);
	}
}