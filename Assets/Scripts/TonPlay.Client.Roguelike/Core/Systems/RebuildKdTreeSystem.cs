using Leopotam.EcsLite;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class RebuildKdTreeSystem : IEcsRunSystem
	{
		private readonly KdTreeStorage[] _storages;
		
		public RebuildKdTreeSystem(params KdTreeStorage[] storages)
		{
			_storages = storages;
		}
		
		public void Run(EcsSystems systems)
		{
			for (var i = 0; i < _storages.Length; i++)
			{
				var storage = _storages[i];

				if (storage.Changed)
				{
					storage.Rebuild();
				}
			}
		}
	}
}