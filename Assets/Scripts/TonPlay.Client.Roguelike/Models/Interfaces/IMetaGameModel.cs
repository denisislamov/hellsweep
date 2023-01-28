using TonPlay.Client.Roguelike.Models.Data;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IMetaGameModel : IModel<MetaGameData>
	{
		IProfileModel ProfileModel { get; }
	}
}