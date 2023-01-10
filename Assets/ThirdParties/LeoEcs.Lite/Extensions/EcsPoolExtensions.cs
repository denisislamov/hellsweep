namespace Leopotam.EcsLite.Extensions
{
	public static class EcsPoolExtensions
	{
		public static ref T AddOrGet<T>(this EcsPool<T> pool, int entity) where T : struct
		{
			if (pool.Has(entity))
			{
				return ref pool.Get(entity);
			}
			
			return ref pool.Add(entity);
		}
	}
}