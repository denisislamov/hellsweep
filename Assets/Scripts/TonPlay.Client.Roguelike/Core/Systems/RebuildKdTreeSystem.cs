using Leopotam.EcsLite;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class RebuildKdTreeSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage[] _storages;

		//private int _lastStorageIndex = -1;

		public RebuildKdTreeSystem(params KdTreeStorage[] storages)
		{
			_storages = storages;
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			for (var i = 0; i < _storages.Length; i++)
			{
				var storage = _storages[i];
				storage.Rebuild();
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}