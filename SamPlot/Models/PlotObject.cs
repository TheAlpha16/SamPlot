using ScottPlot;

namespace Samplot.Models;

public enum PlotType
{
    Line,
    Scatter,
    CSV,
    Function
}

public partial class PlotObject
{
    public Guid ID { get; } = Guid.NewGuid();
    public string Function { get; set; } 
    public double[]? WaveParams { get; set; }

    // public string Color { get; set; }
    public PlotType Type { get; set; }
    public double[]? Xs { get; set; }
    public double[]? Ys { get; set; }

    public string FileName { get; set; }

    public PlotObject(PlotType plotType)
    {
        Type = plotType;
        Random random = new();
        float min = 0;
        float max = 5;

        double frequency = min + (3 - min) * random.NextDouble();
        double amplitude = min + (max - min) * random.NextDouble();
        double phase = min + (max - min) * random.NextDouble();
        double offset = min + (max - min) * random.NextDouble();

        WaveParams = new double[] { frequency, amplitude, phase, offset };
        Function = $"{offset:F1} + {amplitude:F1} sin({2 * frequency:F1}πx + {phase:F1})";
    }

    public PlotObject() {
        Function = $"{FileName}";
    }

    public double CalculateSin(double x)
    {
        if (WaveParams == null)
            return 0;

        return Math.Sin((2 * Math.PI * WaveParams[0] * x) + WaveParams[2]) * WaveParams[1] + WaveParams[3];
    }
}