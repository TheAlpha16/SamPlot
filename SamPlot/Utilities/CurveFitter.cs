using Jace;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.LinearAlgebra;

namespace SamPlot.Utilities;

public static class CurveFitter
{
    public static (Dictionary<string, double> fittedParameters, double[] fittedYs) Fit(
        double[] xs,
        double[] ys,
        string equation,
        Dictionary<string, double> initialGuesses)
    {
        var engine = new CalculationEngine();

        string[] parameterNames = initialGuesses.Keys.ToArray();

        var paramIndex = parameterNames
            .Select((name, idx) => new { name, idx })
            .ToDictionary(p => p.name, p => p.idx);

        // Create cost function: takes parameter array and returns sum of squared error
        Func<Vector<double>, double> cost = parameters =>
        {
            double error = 0;
            for (int i = 0; i < xs.Length; i++)
            {
                var vars = new Dictionary<string, double>
                {
                    { "x", xs[i] }
                };

                foreach (var kvp in paramIndex)
                    vars[kvp.Key] = parameters[kvp.Value];

                try
                {
                    double yEval = engine.Calculate(equation, vars);
                    if (double.IsNaN(yEval) || double.IsInfinity(yEval))
                        error += 1e3 + Math.Abs(ys[i]) * 10;

                    double diff = yEval - ys[i];
                    error += diff * diff;
                }
                catch (Exception ex)
                {
                    // Handle the case where the function evaluation fails
                    // This could be due to invalid parameters or other issues
                    Console.WriteLine($"Error evaluating function '{equation}' at x={xs[i]}: {ex.Message}");
                    Console.WriteLine($"Parameters: {string.Join(", ", parameters)}");
                    error += 1e6;
                }
            }
            // DEBUG
            Console.WriteLine($"Params: {string.Join(", ", parameters)} => Error: {error}");
            return error;
        };

        // Build initial parameter vector
        var initial = Vector<double>.Build.Dense(parameterNames.Length);
        for (int i = 0; i < parameterNames.Length; i++)
        {
            if (initialGuesses.TryGetValue(parameterNames[i], out double guess))
                initial[i] = guess;
            else
                initial[i] = 1.0;
        }

        var nm = new NelderMeadSimplex(1e-5, 100000000);
        var result = nm.FindMinimum(ObjectiveFunction.Value(cost), initial);
        var finalParams = result.MinimizingPoint.ToArray();

        var fittedDict = parameterNames
            .Select((name, idx) => new { name, val = finalParams[idx] })
            .ToDictionary(p => p.name, p => p.val);

        double[] fittedYs = xs.Select(x =>
        {
            var vars = new Dictionary<string, double> { { "x", x } };
            foreach (var kvp in fittedDict)
                vars[kvp.Key] = kvp.Value;

            return engine.Calculate(equation, vars);
        }).ToArray();

        return (fittedDict, fittedYs);
    }
}
