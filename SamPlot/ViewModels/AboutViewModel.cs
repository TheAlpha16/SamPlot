using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SamPlot.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    public AboutViewModel() { }

    [RelayCommand]
    Task GoBack() => Shell.Current.GoToAsync("..");
}