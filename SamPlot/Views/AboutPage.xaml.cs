using SamPlot.ViewModels;

namespace SamPlot.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel aboutViewModel)
	{
		InitializeComponent();
		BindingContext = aboutViewModel;
	}
}

