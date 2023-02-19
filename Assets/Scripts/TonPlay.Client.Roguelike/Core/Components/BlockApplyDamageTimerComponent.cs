using System.Collections.Generic;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct BlockApplyDamageTimerComponent
	{
		public Dictionary<string, Dictionary<int, ReactiveProperty<float>>> Blocked;
	}
}