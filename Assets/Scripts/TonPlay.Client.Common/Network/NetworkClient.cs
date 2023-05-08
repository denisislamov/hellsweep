using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
				[RequestType.DELETE] = (path, data) =>
				{
					var request = UnityWebRequest.Delete(path);
					request.downloadHandler = new DownloadHandlerBuffer();
					return request;
				}
			};
		}

		public async UniTask<Response<T>> GetAsync<T>(string path, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.GET, _basePath, path, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}

		public async UniTask<Response<T>> GetAsync<T>(string path, Dictionary<string, string> requestHeaders, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.GET, _basePath, path, requestHeaders, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}

		public async UniTask<Response<T>> PostAsync<T>(string path, object value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.POST, _basePath, path, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}
		
		public async Task<Response<T>> PostAsync<T, U>(string path, U value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.POST, _basePath, path, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}
		
		public async Task<Response<T>> PostAsync<T, U>(string path, Dictionary<string, string> requestHeaders, U value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.POST, _basePath, path, requestHeaders, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}

		public async Task<Response<T>> PutAsync<T>(string path, object value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.PUT, _basePath, path, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}
		
		public async Task<Response<T>> PutAsync<T, U>(string path, U value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.PUT, _basePath, path, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}
		
		public async Task<Response<T>> PutAsync<T, U>(string path, Dictionary<string, string> requestHeaders, U value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.PUT, _basePath, path, requestHeaders, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}

		public async Task<Response<T>> DeleteAsync<T>(string path, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.DELETE, _basePath, path, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}
		
		public async Task<Response<T>> DeleteAsync<T>(string path, Dictionary<string, string> requestHeaders, T value, CancellationToken cancellationToken = default)
		{
			var request = new RequestContext(RequestType.DELETE, _basePath, path, requestHeaders, value, _timeout, _decorators);
			var responseContext = await InvokeRecursive(request, cancellationToken);
			return GenerateResponse<T>(responseContext);
		}

		public async UniTask<ResponseContext> SendAsync(RequestContext context, CancellationToken cancellationToken, Func<RequestContext, CancellationToken, UniTask<ResponseContext>> _)
		{
			var data = context.Value != null ? JsonUtility.ToJson(context.Value) : " ";
			var formData = new Dictionary<string, string> {{"body", data}};
			Utilities.Logger.Log($"body:\n {data}");
			
			using (var req = _mapRequests[context.RequestType].Invoke(_basePath + context.Path, data))
			{
				Utilities.Logger.Log($"_basePath + context.Path {(_basePath + context.Path)}");
				var headers = context.GetRawHeaders();

				if (headers != null)
				{
					foreach (var item in headers)
					{
						Utilities.Logger.Log($"header: {item.Key} , {item.Value}");
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
				catch (UnityWebRequestException webRequestException)
				{
					Debug.LogError($"{_basePath + context.Path}\t" + webRequestException.ToString());
				}
				finally
				{
					if (!linkToken.IsCancellationRequested)
					{
						linkToken.Cancel();
					}
				}

				Utilities.Logger.Log($"req.downloadHandler.data:\n {req.downloadHandler.data}");
				return new ResponseContext(req.downloadHandler.data, req.responseCode, req.GetResponseHeaders());
			}
		}
		
		private UniTask<ResponseContext> InvokeRecursive(RequestContext context, CancellationToken cancellationToken)
		{
			return context.GetNextDecorator().SendAsync(context, cancellationToken, _next);
		}
		
		private Response<T> GenerateResponse<T>(ResponseContext responseContext)
		{
			var successful = responseContext.StatusCode == (int)HttpStatusCode.OK;
			var response = new Response<T>()
			{
				successful = successful
			};

			if (successful)
			{
				response.response = responseContext.GetResponseAs<T>();
			}
			else
			{
				response.error = responseContext.GetResponseAs<ErrorResponse>();
			}

			return response;
		}
	}
}