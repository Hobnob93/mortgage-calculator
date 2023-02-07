using Microsoft.AspNetCore.Components;
using MortgageCalculator.Components.Enums;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Interfaces;
using MudBlazor;
using System.Net;

namespace MortgageCalculator.Components.BaseComponents;

public class ApiComponentBase : ComponentBase
{
    [Inject]
    protected IWebApiRequest WebRequest { get; private set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    protected ApiComponentState State { get; private set; }

    protected async Task<T?> ApiGet<T>(ApiEndpoint endpoint, params object[] parameters)
    {
        try
        {
            State = ApiComponentState.Loading;
            var result = await WebRequest.GetAsync<T>(endpoint, parameters);
            State = ApiComponentState.Loaded;

            return result;
        }
        catch (Exception ex)
        {
            State = ApiComponentState.Errored;

            Snackbar.Add($"Could not fetch '{endpoint}' data: {ex.Message}", Severity.Error);

            return default;
        }
    }
}
