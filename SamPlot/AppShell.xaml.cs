using SamPlot.Views;

namespace SamPlot;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
		Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
		Routing.RegisterRoute(nameof(PlotPage), typeof(PlotPage));
		// Routing.RegisterRoute(nameof(WaveParamsPage), typeof(WaveParamsPage));

		_ = GoToAsync(nameof(HomePage));
	}
}
