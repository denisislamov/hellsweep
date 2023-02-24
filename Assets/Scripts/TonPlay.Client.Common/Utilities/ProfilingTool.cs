namespace TonPlay.Client.Common.Utilities
{
	public static class ProfilingTool
	{
		public static void BeginSample(string name)
		{
#if ENABLE_PROFILE_SAMPLING
			UnityEngine.Profiling.Profiler.BeginSample(name);
#endif
		}
		
		public static void EndSample()
		{
#if ENABLE_PROFILE_SAMPLING
			UnityEngine.Profiling.Profiler.EndSample();
#endif
		}
		public static void BeginSample(object obj)
		{
#if ENABLE_PROFILE_SAMPLING
			UnityEngine.Profiling.Profiler.BeginSample(obj.GetType().FullName);
#endif
		}
	}
}