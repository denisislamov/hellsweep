using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models
{
	public class MetaGameModelProvider : IMetaGameModelProvider
	{
		private readonly IMetaGameModel _metaGameModel = new MetaGameModel();

		public IMetaGameModel Get() => _metaGameModel;
	}
}