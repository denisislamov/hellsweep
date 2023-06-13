using UnityEngine;

namespace TonPlay.Client.Roguelike.Core
{
	internal abstract class GameControllerExtension : MonoBehaviour, IGameControllerExtension
	{
		protected IGameController GameController;
		
		public void Setup(GameController gameController)
		{
			GameController = gameController;
		}

		public abstract void OnInit();

		public abstract void OnUpdate();

		public abstract void OnFixedUpdate();
	}
}