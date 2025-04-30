using Samplot.Models;

namespace SamPlot.Views;

public partial class NewFunctionDialog : ContentPage
{
    private TaskCompletionSource<PlotObject?> _tcs = new();
    private PlotObject? _existingPlot;
    public bool WasDeleted { get; private set; } = false;


    public NewFunctionDialog(PlotObject? plot = null)
    {
        InitializeComponent();
        _existingPlot = plot;

        if (_existingPlot != null)
        {
            DeleteButton.IsVisible = true;

            FunctionEntry.Text = _existingPlot.Function;
            LabelEntry.Text = _existingPlot.Label;
            XLabelEntry.Text = _existingPlot.XLabel;
            YLabelEntry.Text = _existingPlot.YLabel;

            if (_existingPlot.XRange.HasValue)
            {
                XStartEntry.Text = _existingPlot.XRange.Value.Start.ToString();
                XEndEntry.Text = _existingPlot.XRange.Value.End.ToString();
            }

            if (_existingPlot.Type == PlotType.CSV)
            {
                FunctionEntry.IsEnabled = false;
                XLabelEntry.IsEnabled = false;
                YLabelEntry.IsEnabled = false;
                XStartEntry.IsEnabled = false;
                XEndEntry.IsEnabled = false;
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

        try
        {
            var engine = new Jace.CalculationEngine();
            var testVars = new Dictionary<string, double> { { "x", 0 } };
            engine.Calculate(FunctionEntry.Text.Trim(), testVars); // if this fails, it's invalid
        }
        catch (Exception ex)
        {
            DisplayAlert("Invalid Function", $"Unable to parse or evaluate the function: {ex.Message}", "OK");
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

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        WasDeleted = true;
        _tcs.SetResult(null); 
        Navigation.PopModalAsync();
    }
}
