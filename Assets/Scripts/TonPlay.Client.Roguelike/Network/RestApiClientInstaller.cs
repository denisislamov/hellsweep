using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Network.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Network
{
	public class RestApiClientInstaller : MonoInstaller<RestApiClientInstaller>
	{
		[SerializeField]
		private RestApiWrapper _restApiWrapper;
		
		public override void InstallBindings()
		{
			Container.Bind<IRestApiClient>().FromInstance(_restApiWrapper).AsSingle();
			Container.Bind<IUriProvider>().FromInstance(_restApiWrapper).AsSingle();
		}
	}
}