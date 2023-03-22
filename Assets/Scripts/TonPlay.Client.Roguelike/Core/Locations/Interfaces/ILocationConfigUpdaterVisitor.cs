namespace TonPlay.Client.Roguelike.Core.Locations.Interfaces
{
	public interface ILocationConfigUpdaterVisitor
	{
		void Visit(LocationConfig locationConfig);
	}
}