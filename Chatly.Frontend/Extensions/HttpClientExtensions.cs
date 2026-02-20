using System.Net.Http.Json;

namespace Chatly.Frontend.Extensions;

public static class HttpClientExtensions
{
    private const string IdempotencyKeyHeader = "Idempotency-Key";

    public static Task<HttpResponseMessage> PostAsJsonAsyncWithIdempotency<T>(
        this HttpClient client,
        string requestUri,
        T value,
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        // Create HTTP request message.
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = JsonContent.Create(value)
        };
        
        // Add custom header.
        request.Headers.Add(IdempotencyKeyHeader, idempotencyKey);
        
        // Send the request through HttpClient
        return client.SendAsync(request, cancellationToken);
    }
}