using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Core.Collectables.Interfaces
{
	public interface ICollectableEntityFactory : IFactory<EcsWorld, ICollectableConfig, Vector2, EcsEntity>
	{
	}
}