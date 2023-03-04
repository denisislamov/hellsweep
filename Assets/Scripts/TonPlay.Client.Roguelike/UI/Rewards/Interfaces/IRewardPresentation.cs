using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Rewards.Interfaces
{
	public interface IRewardPresentation
	{
		string Id { get; }
		
		Sprite Icon { get; }
	}
}