using TonPlay.Client.Roguelike.Core.Interfaces;

namespace TonPlay.Client.Roguelike.Core
{
	public class SharedDataProvider : ISharedDataProvider
	{
		private ISharedData _sharedData;

		public ISharedData SharedData => _sharedData;

		public void SetSharedData(ISharedData data)
		{
			_sharedData = data;
		}
	}
}