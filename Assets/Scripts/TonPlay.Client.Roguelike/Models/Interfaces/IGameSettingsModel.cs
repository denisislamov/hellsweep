using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
    public interface IGameSettingsModel : IModel<GameSettingsData>
    {
        IReadOnlyReactiveProperty<float> SoundsVolume { get; }
        IReadOnlyReactiveProperty<float> MusicVolume { get; }
        IReadOnlyReactiveProperty<bool> ScreenGameStick { get; }
        IReadOnlyReactiveProperty<bool> VisualizeDamage { get; }
    }
}