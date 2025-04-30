using System.Text.RegularExpressions;

namespace SamPlot.Utilities;

public static class CurveFittingParser
{
    /// <summary>
    /// Extracts parameter names from a function string (excluding 'x').
    /// </summary>
    public static List<string> ExtractParameters(string function)
    {
        var matches = Regex.Matches(function, @"[a-zA-Z_][a-zA-Z0-9_]*");

        var reserved = new HashSet<string> { "x", "sin", "cos", "tan", "exp", "log", "sqrt", "abs", "pow", "ln", "pi", "e" };
        var parameters = matches
            .Select(m => m.Value)
            .Where(name => !reserved.Contains(name.ToLower()))
            .Distinct()
            .ToList();

        return parameters;
    }

    /// <summary>
    /// Parses an initial guess string like "a=1, b=0, c=1.2"
    /// </summary>
    public static Dictionary<string, double> ParseInitialGuesses(string? guessInput, List<string> parameterNames)
    {
        var result = new Dictionary<string, double>();
        foreach (var name in parameterNames)
            result[name] = 1.0;

        if (string.IsNullOrWhiteSpace(guessInput))
            return result;

        var pairs = guessInput.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) continue;

            var key = parts[0].Trim();
            var val = parts[1].Trim();

            if (parameterNames.Contains(key) && double.TryParse(val, out double parsed))
            {
                result[key] = parsed;
            }
        }

        return result;
    }
}
