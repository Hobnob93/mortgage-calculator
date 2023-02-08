using MortgageCalculator.Components.Interfaces;
using MortgageCalculator.Core.Models;
using MudBlazor;
using System.Reflection;

namespace MortgageCalculator.Components.Services;

public class IconFinder : IIconFinder
{
    public async Task<IconData[]> GetSelectableIcons()
    {
        return await Task.Run(() =>
        {
            var brandIconsInstance = new Icons.Custom.Brands();
            var brandIcons = GetNonObsoleteProperties(brandIconsInstance.GetType())
                .Select(p => (FieldName: p.Name, FieldValue: p.GetValue(brandIconsInstance) as string))
                .Where(d => !string.IsNullOrWhiteSpace(d.FieldValue))
                .Select(d => new IconData
                {
                    Icon = d.FieldValue!,
                    IconName = d.FieldName
                });

            var generalIconsInstance = new Icons.Material.Filled();
            var generalIcons = GetNonObsoleteProperties(generalIconsInstance.GetType())
                .Select(p => (FieldName: p.Name, FieldValue: p.GetValue(brandIconsInstance) as string))
                .Where(d => !string.IsNullOrWhiteSpace(d.FieldValue))
                .Select(d => new IconData
                {
                    Icon = d.FieldValue!,
                    IconName = d.FieldName
                });

            return brandIcons.Union(generalIcons)
                .OrderBy(i => i.IconName)
                .ToArray();
        });
    }

    private IEnumerable<FieldInfo> GetNonObsoleteProperties(Type iconsType)
    {
        return iconsType.GetFields()
            .Where(p => !p.IsDefined(typeof(ObsoleteAttribute), inherit: false));
    }
}
