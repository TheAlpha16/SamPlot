using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SamPlot.Views;

namespace SamPlot.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public HomeViewModel() { }

    [RelayCommand]
    Task NavigateToAbout() => Shell.Current.GoToAsync(nameof(AboutPage));

    [RelayCommand]
    Task NavigateToPlot() => Shell.Current.GoToAsync(nameof(PlotPage));
}
