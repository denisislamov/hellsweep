using System;
using Zenject;

namespace TonPlay.Client.Common.Utilities
{
	public static class SignalBusExtensions
	{
		public static IDisposable SubscribeAsDisposable<T>(this SignalBus signalBus, Action callback)
		{
			return new DisposableSignal<T>(signalBus, callback);
		}

		public static IDisposable SubscribeAsDisposable<T>(this SignalBus signalBus, Action<T> callback)
		{
			return new DisposableSignal<T>(signalBus, callback);
		}
	}
}