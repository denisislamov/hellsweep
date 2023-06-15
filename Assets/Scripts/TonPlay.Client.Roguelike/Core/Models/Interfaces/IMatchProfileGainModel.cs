using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models.Interfaces
{
	public interface IMatchProfileGainModel
	{
		IReadOnlyReactiveProperty<int> Gold { get; }

		IReadOnlyReactiveProperty<float> ProfileExperience { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsArms { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsBody { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsBelt { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsFeet { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsNeck { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsWeapon { get; }
		
		IReadOnlyList<IInventoryItemModel> Items { get; }

		void Update(MatchProfileGainData data);

		MatchProfileGainData ToData();
	}
}