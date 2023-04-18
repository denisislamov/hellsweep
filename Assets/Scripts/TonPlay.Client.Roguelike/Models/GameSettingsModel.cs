using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
    public class GameSettingsModel : IGameSettingsModel
    {
        private readonly GameSettingsData _cached = new GameSettingsData();

        private readonly ReactiveProperty<float> _soundsVolume = new ReactiveProperty<float>();
        private readonly ReactiveProperty<float> _musicVolume = new ReactiveProperty<float>();
        private readonly ReactiveProperty<bool> _screenGameStick = new ReactiveProperty<bool>();
        private readonly ReactiveProperty<bool> _visualizeDamage = new ReactiveProperty<bool>();

        public IReadOnlyReactiveProperty<float> SoundsVolume => _soundsVolume;
        public IReadOnlyReactiveProperty<float> MusicVolume => _musicVolume;
        public IReadOnlyReactiveProperty<bool> ScreenGameStick => _screenGameStick;
        public IReadOnlyReactiveProperty<bool> VisualizeDamage => _visualizeDamage;
		
        public void Update(GameSettingsData data)
        {
            _soundsVolume.Value = data.SoundsVolume;
            _musicVolume.Value = data.MusicVolume;
			
            _screenGameStick.Value = data.ScreenGameStick;
            _visualizeDamage.Value = data.VisualizeDamage;
        }
		
        public GameSettingsData ToData()
        {
            _cached.SoundsVolume = _soundsVolume.Value;
            _cached.MusicVolume = _musicVolume.Value;
			
            _cached.ScreenGameStick = _screenGameStick.Value;
            _cached.VisualizeDamage = _visualizeDamage.Value;
			
            return _cached;
        }
    }
}