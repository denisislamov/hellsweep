using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TonPlay.Client.Common.Network.Interfaces
{
	public interface IAsyncDecorator
	{
		UniTask<ResponseContext> SendAsync(RequestContext context, CancellationToken cancellationToken, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next);
	}
}