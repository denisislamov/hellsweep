using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Loading;
using TonPlay.Roguelike.Client.UI.UIService.Layers;
using UnityEngine;
using Screen = UnityEngine.Screen;

namespace TonPlay.Client.Roguelike.AppEntryPoint
{
	public class MainAppEntryPoint : SceneLoadingAppEntryPoint
	{
		private readonly IUIService _uiService;
		private readonly IUserLoadingService _userLoadingService;
		private readonly IConfigsLoadingService _configsLoadingService;
		private readonly IAnalyticsServiceWrapper _analyticsServiceWrapper;
		private readonly IRestApiClient _restApiClient;
		private readonly ITelegramPlatformProvider _telegramPlatformProvider;
		
		public MainAppEntryPoint(
			ISceneService sceneService,
			string sceneName,
			IUIService uiService,
			IUserLoadingService userLoadingService,
			IConfigsLoadingService configsLoadingService,
			IAnalyticsServiceWrapper analyticsServiceWrapper,
			IRestApiClient restApiClient, 
			ITelegramPlatformProvider telegramPlatformProvider) : base(sceneService, sceneName)
		{
			_uiService = uiService;
			_userLoadingService = userLoadingService;
			_configsLoadingService = configsLoadingService;
			_analyticsServiceWrapper = analyticsServiceWrapper;
			_restApiClient = restApiClient;
			_telegramPlatformProvider = telegramPlatformProvider;
		}

		public override async UniTask ProcessEntrance()
        {
            await base.ProcessEntrance();	
			
			var loadingScreen = _uiService.Open<LoadingScreen, IScreenContext>(ScreenContext.Empty, false, new LoadingScreenLayer());
			
			await _configsLoadingService.Load();

			await _userLoadingService.Load();

            _uiService.Open<MainMenuScreen, MainMenuScreenContext>(new MainMenuScreenContext());
						
			_uiService.Close(loadingScreen);
			
			var gamePropertiesResponse = await _restApiClient.GetGameProperties();
			if (gamePropertiesResponse?.response.jsonData != null)
			{
				var gamePropertiesResponseCache = gamePropertiesResponse.response.jsonData;
				
				Debug.LogFormat("gamePropertiesResponseCache.applicationLaunchedNotFirstTime {0}", 
					gamePropertiesResponseCache.applicationLaunchedNotFirstTime);
				if (!gamePropertiesResponseCache.applicationLaunchedNotFirstTime)
				{
					_analyticsServiceWrapper.OnFirstAppLaunch();
					gamePropertiesResponseCache.applicationLaunchedNotFirstTime = true;
					
					gamePropertiesResponse.response.jsonData = gamePropertiesResponseCache;
					var postGamePropertiesResponse = await _restApiClient.PostGameProperties(gamePropertiesResponse.response);
					if (postGamePropertiesResponse == null)
					{
					    // 	Debug.LogWarning("Can't set GamePropertiesResponse to server");
					}
				}
			}
			else
			{
				// Debug.LogWarning("Can't get GamePropertiesResponse from server");
			}
			
			var userSummaryResponse = await _restApiClient.GetUserSummary();
			
			var theTime = DateTime.Now;
			var date = theTime.ToString("dd/MM/yy");
			
			_analyticsServiceWrapper.OnAppLaunch(userSummaryResponse.response.identifier, 
												 date, 
												 Application.version, 
												 _telegramPlatformProvider.Current.ToString(),
												 SystemInfo.deviceModel,
												 Application.systemLanguage.ToString(), 
												 (Screen.height / (float) (Screen.width) * 9) + ":9" ,
												 "none");
		}
    }
}