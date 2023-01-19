using TonPlay.Client.Roguelike.Core.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models.Interfaces
{
	public interface IMatchProfileGainModel
	{
		IReadOnlyReactiveProperty<int> Gold { get; }
		
		IReadOnlyReactiveProperty<float> ProfileExperience { get; }
		
		void Update(MatchProfileGainData data);
		
		MatchProfileGainData ToData();
	}
}