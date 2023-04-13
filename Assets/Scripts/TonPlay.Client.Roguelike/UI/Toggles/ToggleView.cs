using System;
using TonPlay.Client.Roguelike.UI.Toggles.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Toggles
{
    public class ToggleView : View, IToggleView
    {
        [SerializeField] private Toggle _toggle;
        
        public void SetToggleValue(bool value)
        {
            _toggle.isOn = value;
        }

        public IObservable<bool> OnValueChanged => _toggle.OnValueChangedAsObservable();
    }
}