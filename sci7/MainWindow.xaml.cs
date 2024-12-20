﻿using sci7.Model;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sci7
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateWavesButton_Click(object sender, RoutedEventArgs e)
        {
            WaveformContainer.Children.Clear();
            var random = new Random();

            Color[] colors = {
                Colors.Blue,
                Colors.Red,
                Colors.Green,
                Colors.Orange,
                Colors.Purple,
                Colors.Yellow,
                Colors.Aqua,
                Colors.Cyan,
                Colors.Magenta,
                Colors.Brown,
                Colors.Gray,
                Colors.Pink,
                Colors.LightGreen,
                Colors.LightBlue,
                Colors.LightCoral
            };

            for (int i = 0; i < 15; i++)
            {
                var surface = new SciChartSurface
                {
                    Height = 100,
                    Margin = new Thickness(0, 5, 0, 5),
                    Background = new SolidColorBrush(Colors.Black),
                    XAxis = new NumericAxis(),
                    YAxis = new NumericAxis()
                };

                var xValues = new List<double>();
                var yValues = new List<double>();
                for (int j = 0; j < 100; j++)
                {
                    xValues.Add(j);
                    yValues.Add(random.NextDouble() * 10);
                }

                var seriesName = $"Wave {i + 1}"; // نام موج
                var dataSeries = new XyDataSeries<double, double> { SeriesName = seriesName };
                dataSeries.Append(xValues, yValues);

                var colorIndex = random.Next(colors.Length);
                var renderableSeries = new FastLineRenderableSeries
                {
                    DataSeries = dataSeries,
                    Stroke = colors[colorIndex],
                    StrokeThickness = 3
                };

                surface.RenderableSeries.Add(renderableSeries);
                surface.XAxis.VisibleRange = new DoubleRange(0, 100);
                surface.YAxis.VisibleRange = new DoubleRange(0, 10);

                // اضافه کردن TextBlock برای نمایش نام موج
                var waveNameTextBlock = new TextBlock
                {
                    Text = seriesName, // استفاده از نام موج
                    Foreground = Brushes.DarkGray,
                    FontSize = 14,
                    Margin = new Thickness(5)
                };

                var container = new StackPanel();
                container.Children.Add(waveNameTextBlock);
                container.Children.Add(surface);

                WaveformContainer.Children.Add(container);
                surface.InvalidateVisual();
            }
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WaveformContainer.Children.Count == 0)
                {
                    MessageBox.Show("No waves have been generated to save. Please generate waves first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // خروج از متد
                }

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    DefaultExt = ".json",
                    FileName = $"Waves_{DateTime.Now:yyyyMMdd}.json" // Default name with timestamp
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var wavesData = new List<WaveData>();
                    foreach (UIElement element in WaveformContainer.Children)
                    {
                        var stackPanel = element as StackPanel;
                        if (stackPanel != null)
                        {
                            var surface = stackPanel.Children.OfType<SciChartSurface>().FirstOrDefault();
                            var waveNameTextBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                            if (surface != null && waveNameTextBlock != null)
                            {
                                var series = surface.RenderableSeries[0] as FastLineRenderableSeries;
                                var dataSeries = series.DataSeries as XyDataSeries<double, double>;
                                wavesData.Add(new WaveData
                                {
                                    XValues = dataSeries.XValues.ToArray(),
                                    YValues = dataSeries.YValues.ToArray(),
                                    Color = ColorToHex(series.Stroke),
                                    SeriesName = waveNameTextBlock.Text // ذخیره نام موج
                                });
                            }
                        }
                    }

                    var json = JsonSerializer.Serialize(wavesData);
                    File.WriteAllText(saveFileDialog.FileName, json);
                    MessageBox.Show("Waves saved to " + saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    try
                    {
                        var json = File.ReadAllText(openFileDialog.FileName);
                        var wavesData = JsonSerializer.Deserialize<List<WaveData>>(json);

                        if (wavesData != null)
                        {
                            WaveformContainer.Children.Clear();
                            foreach (var wave in wavesData)
                            {
                                var surface = new SciChartSurface
                                {
                                    Height = 100,
                                    Margin = new Thickness(0, 5, 0, 5)
                                };

                                surface.XAxis = new NumericAxis();
                                surface.YAxis = new NumericAxis();

                                if (wave.XValues != null && wave.YValues != null)
                                {
                                    var dataSeries = new XyDataSeries<double, double> { SeriesName = wave.SeriesName }; // استفاده از نام موج ذخیره شده
                                    dataSeries.Append(wave.XValues, wave.YValues);

                                    var renderableSeries = new FastLineRenderableSeries
                                    {
                                        DataSeries = dataSeries,
                                        Stroke = (Color)ColorConverter.ConvertFromString(wave.Color),
                                        StrokeThickness = 3
                                    };

                                    surface.RenderableSeries.Add(renderableSeries);
                                    surface.XAxis.VisibleRange = new DoubleRange(0, 100);
                                    surface.YAxis.VisibleRange = new DoubleRange(0, 10);

                                    // ایجاد TextBlock برای نمایش نام موج
                                    var waveNameTextBlock = new TextBlock
                                    {
                                        Text = wave.SeriesName, // استفاده از نام موج ذخیره شده
                                        Foreground = Brushes.Gray,
                                        FontSize = 14,
                                        Margin = new Thickness(5) // حاشیه
                                    };

                                    // ایجاد یک StackPanel برای قرار دادن TextBlock و SciChartSurface
                                    var container = new StackPanel();
                                    container.Children.Add(waveNameTextBlock); // اضافه کردن TextBlock
                                    container.Children.Add(surface); // اضافه کردن نمودار

                                    // اضافه کردن StackPanel به WaveformContainer
                                    WaveformContainer.Children.Add(container);
                                }
                            }
                            MessageBox.Show("Waves loaded from " + openFileDialog.FileName);
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        MessageBox.Show("Error loading JSON data: " + jsonEx.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Waves file not found.");
                }
            }
        }

        private void ClearScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (WaveformContainer.Children.Count == 0)
            {
                MessageBox.Show("No waves have been generated. Please generate waves first.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return; // خروج از متد
            }
            WaveformContainer.Children.Clear();
        }

        private string ColorToHex(Color color)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
