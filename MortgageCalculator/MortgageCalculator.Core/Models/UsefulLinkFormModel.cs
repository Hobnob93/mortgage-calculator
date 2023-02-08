using System.ComponentModel.DataAnnotations;

namespace MortgageCalculator.Core.Models;

public class UsefulLinkFormModel
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "You have not provided a name for the link")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "A link needs to be supplied")]
    public string Href { get; set; } = string.Empty;

    [Required(ErrorMessage = "You need to choose an icon to go next to the link")]
    public string IconName { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;
}
