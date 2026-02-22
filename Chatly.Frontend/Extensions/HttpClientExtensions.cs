using System.Net.Http.Json;
using Chatly.Shared.Constants;

namespace Chatly.Frontend.Extensions;

public static class HttpClientExtensions
{
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
        request.Headers.Add(HttpHeaders.IdempotencyKey, idempotencyKey);
        
        // Send the request through HttpClient
        return client.SendAsync(request, cancellationToken);
    }
}