using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TonPlay.Client.Common.Network
{
	public class ResponseContext
	{
		readonly byte[] bytes;
 
		public long StatusCode { get; }
		public Dictionary<string, string> ResponseHeaders { get; }
 
		public ResponseContext(byte[] bytes, long statusCode, Dictionary<string, string> responseHeaders)
		{
			this.bytes = bytes;
			StatusCode = statusCode;
			ResponseHeaders = responseHeaders;
		}
 
		public byte[] GetRawData() => bytes;
 
		public T GetResponseAs<T>()
		{
			var data = Encoding.UTF8.GetString(bytes);

			if (data.Contains("[")) 
			{
				data = "{ \"items\": " + data + "}";
			}
			return JsonUtility.FromJson<T>(data);
		}
	}
}