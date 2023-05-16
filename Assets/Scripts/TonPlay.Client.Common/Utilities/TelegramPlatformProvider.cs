using Zenject;

namespace TonPlay.Client.Common.Utilities
{
	public class TelegramPlatformProvider : ITelegramPlatformProvider
	{
		private const string PLATFORM_QUERY_KEY = "platform";
		
		private readonly IUriProvider _uriProvider;
		
		public TelegramPlatformProvider(IUriProvider uriProvider)
		{
			_uriProvider = uriProvider;
		}
		
		public TelegramPlatform Current
		{
			get
			{
				if (string.IsNullOrEmpty(_uriProvider.CurrentUri))
				{
					return TelegramPlatform.Unknown;
				}
				
				var parsedQuery = UriParser.Parse(_uriProvider.CurrentUri);

				if (!parsedQuery.ContainsKey(PLATFORM_QUERY_KEY))
				{
					return TelegramPlatform.Unknown;
				}
				
				switch (parsedQuery[PLATFORM_QUERY_KEY])
				{
					case "ios":
						return TelegramPlatform.iOS;
					case "android":
						return TelegramPlatform.Android;
					case "macos":
						return TelegramPlatform.MacOS;
					case "webz":
						return TelegramPlatform.WebZ;
					case "tdesktop":
						return TelegramPlatform.Desktop;
				}
				
				return TelegramPlatform.Unknown;
			}
		}
	}
}