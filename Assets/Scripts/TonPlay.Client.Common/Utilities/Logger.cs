using UnityEngine;

namespace TonPlay.Client.Common.Utilities
{
	public static class Logger
	{
		public static bool Enabled => true;

		public static void Log(string text)
		{
#if ENABLE_LOGGER
			Debug.Log(text);
#endif
		}		
		
		public static void LogWarning(string text)
		{
#if ENABLE_LOGGER
			Debug.LogWarning(text);
#endif
		}		
		
		public static void LogError(string text)
		{
#if ENABLE_LOGGER
			Debug.LogError(text);
#endif
		}
	}
}