using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Network.Interfaces;

namespace TonPlay.Client.Common.Network
{
	public class SetupHeadersDecorator : IAsyncDecorator
	{
		public UniTask<ResponseContext> SendAsync(RequestContext context, CancellationToken cancellationToken, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next)
		{
			context.RequestHeaders["Content-Type"] = "application/json";
			context.RequestHeaders["Access-Control-Allow-Origin"] = "*";
			
			return next(context, cancellationToken);
		}
	}
}