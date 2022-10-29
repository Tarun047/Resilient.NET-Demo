namespace ClientApp;

public static class Program
{
    private static async Task TryFlakyApiCall(IClient client)
    {
        try
        {
            Console.WriteLine($"Trying {client.GetType().Name}");
            var response = await client.CallFlakyEndpoint();
            Console.WriteLine(response);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    
    public static async Task Main()
    {
        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };
        var clients = new IClient[]
        {
            new SimpleClient(httpClient),
            new SomeWhatBetterClient(httpClient, 5, TimeSpan.FromSeconds(15)),
            new ResilientClient(httpClient, 5)
        };

        foreach (var client in clients)
        {
            await TryFlakyApiCall(client);
        }
    }
}

