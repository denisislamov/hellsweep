using System;
using System.Collections.Generic;
using TonPlay.Client.Common.Network.Interfaces;
using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Common.Network
{
	public class RequestContext
	{
		private int _decoratorIndex;
		private readonly IAsyncDecorator[] _decorators;
		private Dictionary<string, string> _headers;
 
		public string BasePath { get; }
		public string Path { get; }
		public object Value { get; }
		public TimeSpan Timeout { get; }
		public DateTimeOffset Timestamp { get; private set; }
		
		public RequestType RequestType { get; private set; }
 
		public IDictionary<string, string> RequestHeaders
		{
			get
			{
				if (_headers == null)
				{
					_headers = new Dictionary<string, string>();
				}
				return _headers;
			}
		}
 
		public RequestContext(RequestType requestType, string basePath, string path, object value, TimeSpan timeout, IAsyncDecorator[] filters)
		{
			this._decoratorIndex = -1;
			RequestType = requestType;
			this._decorators = filters;
			this._headers = new DictionaryExt<string, string>();
			this.BasePath = basePath;
			this.Path = path;
			this.Value = value;
			this.Timeout = timeout;
			this.Timestamp = DateTimeOffset.UtcNow;
		}

		public RequestContext(RequestType requestType, string basePath, string path, Dictionary<string, string> requestHeaders, object value, TimeSpan timeout, IAsyncDecorator[] filters) :
			this (requestType, basePath, path, value, timeout, filters)
		{
			_headers = requestHeaders;
		}
		

		internal Dictionary<string, string> GetRawHeaders() => _headers;
		internal IAsyncDecorator GetNextDecorator() => _decorators[++_decoratorIndex];
 
		public void Reset(IAsyncDecorator currentFilter)
		{
			_decoratorIndex = Array.IndexOf(_decorators, currentFilter);
			if (_headers != null)
			{
				_headers.Clear();
			}
			Timestamp = DateTimeOffset.UtcNow;
		}
	}
}