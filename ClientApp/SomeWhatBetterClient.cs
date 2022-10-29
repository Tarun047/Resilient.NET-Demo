namespace ClientApp;

public class SomeWhatBetterClient : IClient
{
    private readonly HttpClient _client;
    private readonly int _maxRetries;
    private readonly TimeSpan _timeToWait;


    public SomeWhatBetterClient(HttpClient client, int maxRetries, TimeSpan timeToWait)
    {
        _client = client;
        _maxRetries = maxRetries;
        _timeToWait = timeToWait;
    }

    public async Task<string> CallFlakyEndpoint()
    {
        int count = 0;
        while (count < _maxRetries)
        {
            try
            {
                var response = await _client.GetAsync("/api/flaky-endpoint");
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    return message;
                }
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine($"Exception encountered: {exception}");
                Console.WriteLine("Retrying ...");
            }

            count += 1;
            await Task.Delay(_timeToWait);
        }

        throw new ClientException($"Unable to reach the API even after trying {_maxRetries} times");
    }
}

public class ClientException : Exception
{
    public ClientException(string message) : base(message)
    {
    }
}