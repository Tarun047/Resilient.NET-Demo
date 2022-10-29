namespace ClientApp;

public interface IClient
{
    Task<string> CallFlakyEndpoint();
}