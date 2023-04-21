using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Common.Network.Interfaces
{
	public interface INetworkClient
	{
		UniTask<Response<T>> PostAsync<T>(string path, object value, CancellationToken cancellationToken = default);

		UniTask<Response<T>> GetAsync<T>(string path, T value, CancellationToken cancellationToken = default);
	}
}