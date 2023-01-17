using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Network.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using Action = Unity.Plastic.Antlr3.Runtime.Misc.Action;

namespace TonPlay.Client.Common.Network
{
	public class NetworkClient : IAsyncDecorator, INetworkClient
	{
		private readonly Func<RequestContext, CancellationToken, UniTask<ResponseContext>> _next;
		private readonly IAsyncDecorator[] _decorators;
		private readonly TimeSpan _timeout;
		private readonly IProgress<float> _progress;
		private readonly string _basePath;
		private readonly Dictionary<RequestType, PerformRequest> _mapRequests;

		private delegate UnityWebRequest PerformRequest(string path, Dictionary<string, string> formData);

		public NetworkClient(string basePath, TimeSpan timeout, params IAsyncDecorator[] decorators)
			: this(basePath, timeout, null, decorators)
		{
		}

		public NetworkClient(string basePath, TimeSpan timeout, IProgress<float> progress, params IAsyncDecorator[] decorators)
		{
			_next = InvokeRecursive; // setup delegate

			_basePath = basePath;
			_timeout = timeout;
			_progress = progress;
			_decorators = new IAsyncDecorator[decorators.Length + 1];
			Array.Copy(decorators, this._decorators, decorators.Length);
			_decorators[this._decorators.Length - 1] = this;
			_mapRequests = new Dictionary<RequestType, PerformRequest>()
			{
				[RequestType.GET] = (path, formData) => UnityWebRequest.Get(path),
				[RequestType.POST] = UnityWebRequest.Post
			};
		}

		public async UniTask<T> PostAsync<T>(string path, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.POST, _basePath, path, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}
		
		public async UniTask<T> GetAsync<T>(string path, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.GET, _basePath, path, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}

		public async UniTask<ResponseContext> SendAsync(RequestContext context, CancellationToken cancellationToken, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> _)
		{
			var data = JsonUtility.ToJson(context.Value);
			var formData = new Dictionary<string, string> {{"body", data}};

			using (var req = _mapRequests[context.RequestType].Invoke(_basePath + context.Path, formData))
			{
				var header = context.GetRawHeaders();
				if (header != null)
				{
					foreach (var item in header)
					{
						req.SetRequestHeader(item.Key, item.Value);
					}
				}

				var linkToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
				linkToken.CancelAfterSlim(_timeout);
				
				try
				{
					await req.SendWebRequest().ToUniTask(progress: _progress, cancellationToken: linkToken.Token);
				}
				catch (OperationCanceledException)
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						throw new TimeoutException();
					}
				}
				finally
				{
					if (!linkToken.IsCancellationRequested)
					{
						linkToken.Cancel();
					}
				}

				return new ResponseContext(req.downloadHandler.data, req.responseCode, req.GetResponseHeaders());
			}
		}
		
		private UniTask<ResponseContext> InvokeRecursive(RequestContext context, CancellationToken cancellationToken)
		{
			return context.GetNextDecorator().SendAsync(context, cancellationToken, _next);
		}
	}
}