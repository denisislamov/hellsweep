using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models.Data
{
    public class GameSettingsData : IData
    {
        public float SoundsVolume { get; set; }
        public float MusicVolume { get; set; }

        public bool ScreenGameStick { get; set; }
        public bool VisualizeDamage { get; set; }
    }
}