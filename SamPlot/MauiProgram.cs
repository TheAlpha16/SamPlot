using SamPlot.Views;
using SamPlot.ViewModels;
using Samplot.Models;
using ScottPlot.Maui;
using Microsoft.Extensions.Logging;

namespace SamPlot;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseScottPlot()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterAppServices()
			.RegisterModels()
			.RegisterViewModels()
			.RegisterViews();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
	{
		_ = builder.Services.AddSingleton<AppShell>();

		return builder;
	}

	private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
	{
		_ = builder.Services.AddTransient<AboutPage>();
		_ = builder.Services.AddTransient<HomePage>();
		_ = builder.Services.AddTransient<PlotPage>();

		return builder;
	}

	private static MauiAppBuilder RegisterModels(this MauiAppBuilder builder)
	{
		_ = builder.Services.AddTransient<PlotObject>();

		return builder;
	}

	private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
	{
		_ = builder.Services.AddTransient<HomeViewModel>();
		_ = builder.Services.AddTransient<AboutViewModel>();
		_ = builder.Services.AddTransient<PlotViewModel>();

		return builder;
	}
}
