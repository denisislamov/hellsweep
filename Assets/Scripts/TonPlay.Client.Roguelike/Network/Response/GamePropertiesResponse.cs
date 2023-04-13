using System;

namespace TonPlay.Client.Roguelike.Network.Response
{
    [System.Serializable]
    public class GamePropertiesResponse
    {
        public GameProperties gameProperties;
		
        [System.Serializable]
        public class GameProperties
        {
            public JsonData jsonData;

            [System.Serializable]
            public class JsonData
            {
                public GameSettings gameSettings;
                
                [Serializable]
                public class GameSettings
                {
                    public float SoundsVolume;
                    public float MusicVolume;
                    
                    public bool ScreenGameStick;
                    public bool VisualizeDamage;
                }
            }
        }
    }
}