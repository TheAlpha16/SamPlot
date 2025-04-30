using Samplot.Models;

namespace SamPlot.Views;

public partial class NewFunctionDialog : ContentPage
{
    private TaskCompletionSource<PlotObject?> _tcs = new();

    public NewFunctionDialog()
    {
        InitializeComponent();
    }

    public Task<PlotObject?> ShowAsync() => _tcs.Task;

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FunctionEntry.Text) ||
            string.IsNullOrWhiteSpace(LabelEntry.Text) ||
            !double.TryParse(XStartEntry.Text, out double xStart) ||
            !double.TryParse(XEndEntry.Text, out double xEnd))
        {
            DisplayAlert("Error", "Please enter valid function, label, and x-range.", "OK");
            return;
        }

        var plot = new PlotObject
        {
            Type = PlotType.Function,
            Function = FunctionEntry.Text.Trim(),
            Label = LabelEntry.Text.Trim(),
            XLabel = XLabelEntry.Text?.Trim(),
            YLabel = YLabelEntry.Text?.Trim(),
            XRange = (xStart, xEnd)
        };

        _tcs.SetResult(plot);
        Navigation.PopModalAsync();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _tcs.SetResult(null);
        Navigation.PopModalAsync();
    }
}
