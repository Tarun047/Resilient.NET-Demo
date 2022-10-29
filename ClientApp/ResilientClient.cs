using Polly;
using Polly.Contrib.WaitAndRetry;

namespace ClientApp;

public class ResilientClient : IClient
{
    private readonly HttpClient _client;
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;

    public ResilientClient(HttpClient client, int maxRetries)
    {
        _client = client;
        _policy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(response => !response.IsSuccessStatusCode)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(3), maxRetries));

    }

    public async Task<string> CallFlakyEndpoint()
    {
        var response = await _policy.ExecuteAsync(() => _client.GetAsync("/api/flaky-endpoint"));
        var message =  await response.Content.ReadAsStringAsync();
        return message;
    }
}