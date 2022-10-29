namespace ClientApp;

public class SimpleClient : IClient
{
    private readonly HttpClient _client;
    public SimpleClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> CallFlakyEndpoint()
    {
        var response = await _client.GetAsync("/api/flaky-endpoint");
        var message =  await response.Content.ReadAsStringAsync();
        return message;
    }
}