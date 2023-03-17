using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Network.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

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

		private delegate UnityWebRequest PerformRequest(string path, string postData);

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
				[RequestType.GET]    = (path, data) => UnityWebRequest.Get(path),
				[RequestType.POST]   = (path, data) => UnityWebRequest.Post(path, data),
				[RequestType.PUT]    = (path, data) => UnityWebRequest.Put(path, data),
				[RequestType.DELETE] = (path, data) => UnityWebRequest.Delete(path)
			};
		}

		public async UniTask<T> GetAsync<T>(string path, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.GET, _basePath, path, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}

		public async UniTask<T> GetAsync<T>(string path, Dictionary<string, string> requestHeaders, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.GET, _basePath, path, requestHeaders, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}

		public async UniTask<T> PostAsync<T>(string path, object value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.POST, _basePath, path, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}
		
		public async UniTask<T> PostAsync<T, U>(string path, Dictionary<string, string> requestHeaders, U value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.POST, _basePath, path, requestHeaders, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}

		public async UniTask<T> PutAsync<T>(string path, object value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.PUT, _basePath, path, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}
		
		public async UniTask<T> PutAsync<T, U>(string path, Dictionary<string, string> requestHeaders, U value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.PUT, _basePath, path, requestHeaders, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}

		public async UniTask<T> DeleteAsync<T>(string path, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.DELETE, _basePath, path, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}
		
		public async UniTask<T> DeleteAsync<T>(string path, Dictionary<string, string> requestHeaders, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.DELETE, _basePath, path, requestHeaders, value, _timeout, _decorators);
			var response = await InvokeRecursive(request, cancellationToken);
			return response.GetResponseAs<T>();
		}

		public async UniTask<ResponseContext> SendAsync(RequestContext context, CancellationToken cancellationToken, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> _)
		{
			var data = context.Value != null ? JsonUtility.ToJson(context.Value) : " ";
			var formData = new Dictionary<string, string> {{"body", data}};
			Debug.LogFormat("body:\n {0}", data);
			
			using (var req = _mapRequests[context.RequestType].Invoke(_basePath + context.Path, data))
			{
				Debug.LogFormat("_basePath + context.Path {0}", _basePath + context.Path);
				var headers = context.GetRawHeaders();

				if (headers != null)
				{
					foreach (var item in headers)
					{
						Debug.LogFormat("header: {0} , {1}", item.Key, item.Value);
						req.SetRequestHeader(item.Key, item.Value);
					}
				}
				
				// TODO - not sure about it
				if (req.uploadHandler != null)
				{
					req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));
					req.uploadHandler.contentType = "application/json";
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

				Debug.LogFormat("req.downloadHandler.data:\n {0}", req.downloadHandler.data);
				return new ResponseContext(req.downloadHandler.data, req.responseCode, req.GetResponseHeaders());
			}
		}
		
		private UniTask<ResponseContext> InvokeRecursive(RequestContext context, CancellationToken cancellationToken)
		{
			return context.GetNextDecorator().SendAsync(context, cancellationToken, _next);
		}
    }
}