namespace MortgageCalculator;

public partial class MainLayout
{
    private bool _drawerOpen = false;

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }
}
