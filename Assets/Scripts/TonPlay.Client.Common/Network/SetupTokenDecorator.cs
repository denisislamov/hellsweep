using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Network.Interfaces;

namespace TonPlay.Client.Common.Network
{
	public class SetupTokenDecorator : IAsyncDecorator
	{
		private readonly string _token;
		
		public SetupTokenDecorator(string token)
		{
			_token = token;
		}

		public UniTask<ResponseContext> SendAsync(RequestContext context, CancellationToken cancellationToken, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> next)
		{
			context.RequestHeaders["Authorization"] = "Bearer " + _token;

			return next(context, cancellationToken);
		}
	}
}