using System;
using System.Collections.Generic;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.Extensions;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Layers;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Common.UIService
{
    public class UIService : IUIService
    {
        private readonly IDictionary<Type, IUIService> _layeredServices = new Dictionary<Type, IUIService>();

        public UIService(IDefaultScreenRoot defaultScreenRoot, IEnumerable<IScreenLayer> layers, IScreenFactoryFacade screenFactoryFacade)
        {
            if (layers == null) return;

            var screenRoot = defaultScreenRoot.Root;
            
            foreach (var layer in layers)
            {
                var name = layer.GetType().Name;
                var layerGameObject = new GameObject(name);
                layerGameObject.transform.SetParent(screenRoot);
                layerGameObject.transform.Reset();
                layerGameObject.AddComponent<Canvas>();
                layerGameObject.AddComponent<GraphicRaycaster>();
                var rectTransform = layerGameObject.GetComponent<RectTransform>();
                rectTransform.ResetRectSize();
                
                _layeredServices.Add(layer.GetType(), new LayerUIService(screenFactoryFacade, layer, layerGameObject.transform));
            }
        }
        
        public void Open<TScreen, TContext>(TContext context, bool isEmbedded = false, IScreenLayer screenLayer = null) 
            where TScreen : IScreen
            where TContext : IScreenContext
        {
            if (screenLayer is null)
            {
                screenLayer = new DefaultScreenLayer();
            }
            
            _layeredServices[screenLayer.GetType()].Open<TScreen, TContext>(context, isEmbedded);
        }
        
        public void Close(IScreen screen, bool isEmbedded)
        {
            _layeredServices[screen.RootLayer.GetType()].Close(screen, isEmbedded);
        }
        
        public void CloseAll(IScreenLayer layer = null)
        {
            if (layer is null)
            {
                foreach (var kvp in _layeredServices)
                {
                    kvp.Value.CloseAll();
                }
            }
            else
            {
                _layeredServices[layer.GetType()].CloseAll();
            }
        }

        public Transform GetScreensRoot(IScreenLayer screenLayer = null)
        {
            if (screenLayer is null)
            {
                screenLayer = new DefaultScreenLayer();
            }
            
            return _layeredServices[screenLayer.GetType()].GetScreensRoot();
        }
    }
}
