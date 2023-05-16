namespace TonPlay.Client.Common.Utilities
{
	public interface ITelegramPlatformProvider
	{
		TelegramPlatform Current { get; }
	}
}