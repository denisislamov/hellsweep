using TonPlay.Client.Common.Network.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Common.Network
{
	[CreateAssetMenu(fileName = nameof(NetworkSettings), menuName = AssetMenuConstants.NETWORK_SERVICE_INSTALLERS + nameof(NetworkSettings))]
	public class NetworkSettings : ScriptableObject, INetworkSettings
	{
		[SerializeField]
		private string _baseAddress;
		
		[SerializeField]
		private int _port;

		public string Address => string.Format("{0}:{1}/", _baseAddress, _port.ToString());
	}
}