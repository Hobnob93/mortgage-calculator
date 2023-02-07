using Microsoft.Extensions.Options;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Interfaces;
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
        var endpoint = GetEndpoint(apiEndpoint);

        if (parameters is not null && parameters.Length > 0)
            endpoint = string.Format(endpoint, parameters);

        var request = new HttpRequestMessage(HttpMethod.Get, $"{_config.BaseUrl}{endpoint}");
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

    private string GetEndpoint(ApiEndpoint endpoint) =>
        endpoint switch
        {
            ApiEndpoint.UsefulLinks => _config.UsefulLinks,
            _ => throw new InvalidOperationException($"The endpoint {endpoint} has not been defined.")
        };
}
