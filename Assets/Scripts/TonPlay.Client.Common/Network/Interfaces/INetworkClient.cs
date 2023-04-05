using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Common.Network.Interfaces
{
	public interface INetworkClient
	{
		UniTask<T> PostAsync<T>(string path, object value, CancellationToken cancellationToken = default);

		Task<Response<T>> GetAsync<T>(string path, T value, CancellationToken cancellationToken = default);
	}
}