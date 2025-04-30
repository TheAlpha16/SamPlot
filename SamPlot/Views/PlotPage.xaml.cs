using Samplot.Models;
using SamPlot.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

namespace SamPlot.Views;

public partial class PlotPage : ContentPage
{
	private PlotViewModel _viewModel;

	public PlotPage()
	{
		InitializeComponent();

		_viewModel = new PlotViewModel(PlotView);
		BindingContext = _viewModel;

		Shell.SetBackButtonBehavior(
			this, new BackButtonBehavior
			{
				IsEnabled = false,
				IsVisible = false
			});
	}

	private async void Open_CSV_Clicked(object sender, EventArgs e)
	{
		var fileResults = await FilePicker.PickMultipleAsync(new PickOptions
		{
			PickerTitle = "Select CSV File(s)",
			// FileTypes = FilePickerFileType
		});

		if (fileResults != null && fileResults.Any())
		{
			foreach (var fileResult in fileResults)
			{
				try
				{
					string filePath = fileResult.FullPath;
					var lines = File.ReadAllLines(filePath);

					if (lines.Length < 2)
					{
						await DisplayAlert("Error", $"File {fileResult.FileName} does not contain enough data.", "OK");
						continue;
					}

					var headers = lines[0].Split(',');

					var data = lines
						.Skip(1)
						.Select(line => line.Split(','))
						.Select(parts => new
						{
							X = double.Parse(parts[0], CultureInfo.InvariantCulture),
							Y = double.Parse(parts[1], CultureInfo.InvariantCulture)
						})
						.ToList();

					double[] xValues = data.Select(d => d.X).ToArray();
					double[] yValues = data.Select(d => d.Y).ToArray();

					PlotObject plotObject = new()
					{
						Type = PlotType.CSV,
						Xs = xValues,
						Ys = yValues,
						Function = fileResult.FileName,
						Label = fileResult.FileName
					};

					_viewModel.PlotObjects.Add(plotObject);
				}
				catch (Exception ex)
				{
					await DisplayAlert("Error", $"An error occurred while reading file {fileResult.FileName}: {ex.Message}", "OK");
				}
			}
		}
	}
}