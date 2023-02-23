using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IProfileModel : IModel<ProfileData>
	{
		IBalanceModel BalanceModel { get; }

		IReadOnlyReactiveProperty<int> Level { get; }

		IReadOnlyReactiveProperty<float> Experience { get; }

		IReadOnlyReactiveProperty<float> MaxExperience { get; }
	}
}