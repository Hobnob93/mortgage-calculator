using Microsoft.Extensions.Options;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Interfaces;
using System.Text;
using System.Text.Json;

namespace MortgageCalculator.Core.Services;

public class WebApiRequest : IWebApiRequest
{
    private readonly ApiEndpointConfig _config;
    private readonly IHttpClientFactory _clientFactory;

    public WebApiRequest(IOptions<ApiEndpointConfig> config, IHttpClientFactory clientFactory)
    {
        _config = config.Value;
        _clientFactory = clientFactory;
    }

    public async Task<T> GetAsync<T>(ApiEndpoint apiEndpoint, params object[] parameters)
    {
        var client = _clientFactory.CreateClient();
        var uri = GetEndpointUri(apiEndpoint);

        if (parameters is not null && parameters.Length > 0)
            uri = string.Format(uri, parameters);

        var request = new HttpRequestMessage(HttpMethod.Get, $"{_config.BaseUrl}{uri}");
        var response = await client.SendAsync(request);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        response.EnsureSuccessStatusCode();
        using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(stream, options)
            ?? throw new InvalidCastException("Could not deserialize response stream.");
    }

    public async Task PatchAsync(ApiEndpoint apiEndpoint, object parameter)
    {
        var client = _clientFactory.CreateClient();
        var uri = GetEndpointUri(apiEndpoint);

        var request = new HttpRequestMessage(HttpMethod.Patch, $"{_config.BaseUrl}{uri}");

        if (parameter is not null)
            request.Content = new StringContent(JsonSerializer.Serialize(parameter), Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(ApiEndpoint apiEndpoint, object parameter)
    {
        var client = _clientFactory.CreateClient();
        var uri = GetEndpointUri(apiEndpoint);

        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_config.BaseUrl}{uri}");

        if (parameter is not null)
            request.Content = new StringContent(JsonSerializer.Serialize(parameter), Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }

    private string GetEndpointUri(ApiEndpoint endpoint) =>
        endpoint switch
        {
            ApiEndpoint.UsefulLinks => _config.UsefulLinks,
            _ => throw new InvalidOperationException($"The endpoint {endpoint} has not been defined.")
        };
}
