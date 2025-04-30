using Samplot.Models;
using SamPlot.Views;
using ScottPlot.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SamPlot.ViewModels;

public partial class PlotViewModel : ObservableObject
{
    public ObservableCollection<PlotObject> PlotObjects { get; } = new ObservableCollection<PlotObject>();
    private MauiPlot _plotView;

    public ICommand AddPlotCommand { get; }
    public ICommand RemovePlotCommand { get; }

    public PlotViewModel(MauiPlot plot)
    {
        _plotView = plot;
        AddPlotCommand = new Command(() => AddPlot());
        RemovePlotCommand = new Command<PlotObject>(RemovePlot);

        InitializePlot();
        PlotObjects.CollectionChanged += PlotObjects_CollectionChanged;
    }

    private void InitializePlot()
    {
        _plotView.Plot.FigureBackground.Color = new("#1c1c1e");
        _plotView.Plot.Axes.Color(new("#888888"));

        _plotView.Plot.Grid.XAxisStyle.FillColor1 = new ScottPlot.Color("#888888").WithAlpha(10);
        _plotView.Plot.Grid.YAxisStyle.FillColor1 = new ScottPlot.Color("#888888").WithAlpha(10);

        // set grid line colors
        _plotView.Plot.Grid.XAxisStyle.MajorLineStyle.Color = ScottPlot.Colors.White.WithAlpha(15);
        _plotView.Plot.Grid.YAxisStyle.MajorLineStyle.Color = ScottPlot.Colors.White.WithAlpha(15);
        _plotView.Plot.Grid.XAxisStyle.MinorLineStyle.Color = ScottPlot.Colors.White.WithAlpha(5);
        _plotView.Plot.Grid.YAxisStyle.MinorLineStyle.Color = ScottPlot.Colors.White.WithAlpha(5);

        // enable minor grid lines by defining a positive width
        _plotView.Plot.Grid.XAxisStyle.MinorLineStyle.Width = 1;
        _plotView.Plot.Grid.YAxisStyle.MinorLineStyle.Width = 1;
        // _plotView.Plot.Axes.SquareUnits();

        _plotView.Refresh();
    }

    public async void AddPlot()
    {
        var dialog = new NewFunctionDialog();
        await Application.Current.MainPage.Navigation.PushModalAsync(dialog);
        var result = await dialog.ShowAsync();

        if (result != null)
            PlotObjects.Add(result);
    }

    private void RemovePlot(PlotObject plotObject)
    {
        if (plotObject != null && PlotObjects.Contains(plotObject))
        {
            PlotObjects.Remove(plotObject);
        }
    }

    public void PlotObjects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Console.WriteLine("PlotObjects collection changed");
        _plotView.Plot.Clear();
        foreach (var plotObject in PlotObjects)
        {
            Console.WriteLine($"Adding plot object of type {plotObject.Type}");
            switch (plotObject.Type)
            {
                case PlotType.Function:
                    var range = plotObject.XRange ?? (-10, 10);
                    double xStart = range.Item1;
                    double xEnd = range.Item2;
                    double granularity = 0.01;
                    int maxPoints = 500;

                    int count = Math.Min((int)((xEnd - xStart) / granularity), maxPoints);
                    double[] xs = Enumerable.Range(0, count)
                                            .Select(i => xStart + i * ((xEnd - xStart) / count))
                                            .ToArray();
                    Console.WriteLine("xs: " + string.Join(", ", xs));
                    double[] ys = xs.Select(x => plotObject.EvaluateFunction(x)).ToArray();
                    var funcPlot = _plotView.Plot.Add.Scatter(xs, ys);
                    funcPlot.LegendText = plotObject.Label;
                    break;

                case PlotType.Scatter:
                    _plotView.Plot.Add.Scatter(plotObject.Xs, plotObject.Ys);
                    break;

                case PlotType.CSV:
                    var plt = _plotView.Plot.Add.Scatter(plotObject.Xs, plotObject.Ys);
                    plt.LegendText = plotObject.Label;
                    break;
            }
        }
        // Optional: Set axis labels
        var xLabel = PlotObjects.FirstOrDefault(p => p.XLabel != null)?.XLabel;
        var yLabel = PlotObjects.FirstOrDefault(p => p.YLabel != null)?.YLabel;
        if (xLabel != null) _plotView.Plot.Axes.Bottom.Label.Text = xLabel;
        if (yLabel != null) _plotView.Plot.Axes.Left.Label.Text = yLabel;

        _plotView.Plot.Legend.BackgroundColor = new("#1c1c1e");
        _plotView.Plot.Legend.FontColor = new("#e0e0e0");
        _plotView.Plot.Legend.OutlineColor = new("Transparent");
        _plotView.Plot.ShowLegend();

        _plotView.Plot.Axes.AutoScale();
        _plotView.Refresh();
    }
}
