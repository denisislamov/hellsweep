using System;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Client.Roguelike.UI.Sliders .Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Sliders
{
    public class SliderView : View, ISliderView
    {
        [SerializeField] private Slider _slider;
		
        public void SetSliderValue(float value)
        {
            _slider.value = value;
        }

        public IObservable<float> OnValueChanged => _slider.OnValueChangedAsObservable();
    }
}