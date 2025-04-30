namespace SamPlot.Views;

public partial class FitCurveDialog : ContentPage
{
    private TaskCompletionSource<(string? equation, string? guesses)> _tcs = new();

    public FitCurveDialog()
    {
        InitializeComponent();
    }

    public Task<(string? equation, string? guesses)> ShowAsync() => _tcs.Task;

    private void OnFitClicked(object sender, EventArgs e)
    {
        var eq = EquationEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(eq))
        {
            DisplayAlert("Error", "Model equation is required", "OK");
            return;
        }

        _tcs.SetResult((eq, GuessesEntry.Text?.Trim()));
        Navigation.PopModalAsync();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _tcs.SetResult((null, null));
        Navigation.PopModalAsync();
    }
}
