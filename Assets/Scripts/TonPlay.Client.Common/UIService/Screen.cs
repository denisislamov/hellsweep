using System.Collections.Generic;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.Extensions;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Common.UIService
{
	public abstract class Screen : MonoBehaviour, IScreen
	{
		private const string DEFAULT_EMBEDDED_ROOT_NAME = "DefaultEmbeddedScreensRoot";

		[SerializeField]
		private Transform _customEmbeddedScreensRoot;

		public GameObject GameObject => gameObject;
		
		public IScreenLayer RootLayer => _rootLayer;
		
		public IScreenStack EmbeddedScreensStack { get; } = new ScreenStack();

		protected List<IPresenter> Presenters { get; } = new List<IPresenter>();
		
		private Transform _embeddedScreensRoot;
		private IScreenLayer _rootLayer;

		protected void Construct(IScreenLayer screenLayer)
		{
			_rootLayer = screenLayer;
			
			_embeddedScreensRoot = CreateEmbeddedScreensRoot();
		}
		
		private Transform CreateEmbeddedScreensRoot()
		{
			if (_customEmbeddedScreensRoot != null) return _customEmbeddedScreensRoot;

			var root = new GameObject(DEFAULT_EMBEDDED_ROOT_NAME);
			root.transform.SetParent(transform);
			root.transform.Reset();
			root.AddComponent<Canvas>();
			var rectTransform = root.GetComponent<RectTransform>();
			rectTransform.ResetRectSize();

			return root.transform;
		}

		public virtual Transform GetEmbeddedTransformRoot() => _embeddedScreensRoot;

		public virtual void Open()
		{
			foreach (var presenter in Presenters)
			{
				presenter.Show();
			}
		}

		public virtual void Close()
		{
			foreach (var presenter in Presenters)
			{
				presenter.Hide();
			}
		}
		
		public void Dispose()
		{
			foreach (var presenter in Presenters)
			{
				presenter.Dispose();
			}
		}
	}

	public abstract class Screen<TContext> : Screen, IScreen<TContext> 
		where TContext : class, IScreenContext
	{
		protected TContext Context { get; private set; }

		[Inject]
		private void Construct(TContext context, IScreenLayer layer)
		{
			Context = context;
			Context.Screen = this;
			base.Construct(layer);
		}
	}
}