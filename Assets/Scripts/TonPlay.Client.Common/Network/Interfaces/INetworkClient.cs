using System.Threading;
using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Common.Network.Interfaces
{
	public interface INetworkClient
	{
		UniTask<T> PostAsync<T>(string path, object value, CancellationToken cancellationToken = default);

		UniTask<T> GetAsync<T>(string path, T value, CancellationToken cancellationToken = default);
	}
}