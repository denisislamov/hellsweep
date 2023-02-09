using System;
using Zenject;

namespace TonPlay.Client.Common.Utilities
{
	public class DisposableSignal<T> : IDisposable
	{
		private readonly SignalBus _signalBus;
		private readonly Action _callback;
		private readonly Action<T> _callbackWithParam;
        
		public DisposableSignal(SignalBus signalBus, Action callback)
		{
			_signalBus = signalBus;
			_callback = callback;
            
			signalBus.Subscribe<T>(callback);
		}
        
		public DisposableSignal(SignalBus signalBus, Action<T> callback)
		{
			_signalBus = signalBus;
			_callbackWithParam = callback;
            
			signalBus.Subscribe(callback);
		}

		public void Dispose()
		{
			if (_callback != null)
			{
				_signalBus.Unsubscribe<T>(_callback);
			}
            
			if (_callbackWithParam != null)
			{
				_signalBus.Unsubscribe(_callbackWithParam);
			}
		}
	}

}