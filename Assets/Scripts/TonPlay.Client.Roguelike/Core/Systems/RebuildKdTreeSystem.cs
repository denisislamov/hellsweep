using Leopotam.EcsLite;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class RebuildKdTreeSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage[] _storages;

		private int _lastStorageIndex = -1;

		public RebuildKdTreeSystem(params KdTreeStorage[] storages)
		{
			_storages = storages;
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			_lastStorageIndex++;
			_lastStorageIndex %= _storages.Length;
			var storage = _storages[_lastStorageIndex++];
			if (storage.Changed)
			{
				storage.Rebuild();
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}