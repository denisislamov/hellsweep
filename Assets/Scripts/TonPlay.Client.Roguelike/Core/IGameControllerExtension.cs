namespace TonPlay.Client.Roguelike.Core
{
	internal interface IGameControllerExtension
	{
		void OnInit();

		void OnUpdate();

		void OnFixedUpdate();
	}
}