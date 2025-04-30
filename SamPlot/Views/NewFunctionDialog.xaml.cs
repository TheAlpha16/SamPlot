using Samplot.Models;

namespace SamPlot.Views;

public partial class NewFunctionDialog : ContentPage
{
    private TaskCompletionSource<PlotObject?> _tcs = new();
    private PlotObject? _existingPlot;

    public NewFunctionDialog(PlotObject? plot = null)
    {
        InitializeComponent();
        _existingPlot = plot;

        if (_existingPlot != null)
        {
            FunctionEntry.Text = _existingPlot.Function;
            LabelEntry.Text = _existingPlot.Label;
            XLabelEntry.Text = _existingPlot.XLabel;
            YLabelEntry.Text = _existingPlot.YLabel;
            if (_existingPlot.XRange.HasValue)
            {
                XStartEntry.Text = _existingPlot.XRange.Value.Start.ToString();
                XEndEntry.Text = _existingPlot.XRange.Value.End.ToString();
            }
        }
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
