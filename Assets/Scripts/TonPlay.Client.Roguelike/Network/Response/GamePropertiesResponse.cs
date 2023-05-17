using System;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class GamePropertiesResponse
    {
        public JsonData jsonData = new JsonData();

        [System.Serializable]
        public class JsonData
        {
            public GameSettings gameSettings = new GameSettings();

            [Serializable]
            public class GameSettings
            {
                public float SoundsVolume;
                public float MusicVolume;

                public bool ScreenGameStick;
                public bool VisualizeDamage;
            }
            
            public bool applicationLaunchedNotFirstTime;
        }
    }
}