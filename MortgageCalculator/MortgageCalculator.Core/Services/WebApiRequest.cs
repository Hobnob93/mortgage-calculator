using Microsoft.Extensions.Options;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Interfaces;
using MudBlazor;
using System.Text.Json;

namespace MortgageCalculator.Core.Services;

public class WebApiRequest : IWebApiRequest
{
    private readonly ApiEndpointConfig _config;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ISnackbar _snackbar;

    public WebApiRequest(IOptions<ApiEndpointConfig> config, IHttpClientFactory clientFactory, ISnackbar snackbar)
    {
        _config = config.Value;
        _clientFactory = clientFactory;
        _snackbar = snackbar;
    }

    public async Task<T?> GetAsync<T>(ApiEndpoint apiEndpoint, params object[] parameters)
    {
        try
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
        catch (Exception ex)
        {
            _snackbar.Add($"Could not fetch '{apiEndpoint}' data: {ex.Message}", Severity.Error);

            return default;
        }
    }

    private string GetEndpoint(ApiEndpoint endpoint) =>
        endpoint switch
        {
            ApiEndpoint.UsefulLinks => _config.UsefulLinks,
            _ => throw new InvalidOperationException($"The endpoint {endpoint} has not been defined.")
        };
}
