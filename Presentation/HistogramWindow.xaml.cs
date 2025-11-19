using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Algorithms.Histogram;
using Presentation.WindowManagement;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Presentation;

public partial class HistogramWindow : Window
{
    private HistogramData _histogramData;
    private readonly ManagedWindow _parentWindow;
    
    public HistogramWindow(ManagedWindow parentWindow)
    {
        _parentWindow = parentWindow;
        var bitmapSource = _parentWindow.Image.ToBitmapSource();
        InitializeComponent();
        var width = bitmapSource.PixelWidth;
        var height = bitmapSource.PixelHeight;
        var stride = (width * bitmapSource.Format.BitsPerPixel + 7) / 8;
        var pixels = new byte[height * stride];
        bitmapSource.CopyPixels(pixels, stride, 0);

        var bytesPerPixel = (bitmapSource.Format.BitsPerPixel + 7) / 8;

        _histogramData = HistogramCalculator.CalculateGrayscale(pixels, bytesPerPixel);
        
        Loaded += (sender, args) => LoadHistogram();
        Owner = Application.Current.MainWindow;
    }

    private void LoadHistogram()
    {
        try
        {
            DisplayStatistics();

            DrawHistogram();
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error while generating a histogram: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DisplayStatistics()
    {
        MeanText.Text = $"{_histogramData.Mean:F2}";
        StdDevText.Text = $"{_histogramData.StandardDeviation:F2}";
        MedianText.Text = $"{_histogramData.Median:F2}";
        MinText.Text = _histogramData.Min.ToString();
        MaxText.Text = _histogramData.Max.ToString();
        TotalPixelsText.Text = _histogramData.TotalPixels.ToString("N0");
    }

    private void DrawHistogram()
    {
        HistogramCanvas.Children.Clear();

        var canvasWidth = HistogramCanvas.ActualWidth;
        var canvasHeight = HistogramCanvas.ActualHeight;

        if (canvasWidth <= 0 || canvasHeight <= 0) return;

        double margin = 50;
        var chartWidth = canvasWidth - 2 * margin;
        var chartHeight = canvasHeight - 2 * margin;

        var maxFrequency = _histogramData.Frequencies.Max();
        if (maxFrequency == 0) return;

        var barWidth = chartWidth / 256;
        
        DrawAxes(margin, chartWidth, chartHeight, maxFrequency);

        for (var i = 0; i < 256; i++)
        {
            if (_histogramData.Frequencies[i] <= 0) continue;
            var barHeight = (_histogramData.Frequencies[i] / (double)maxFrequency) * chartHeight;
            var x = margin + i * barWidth;
            var y = margin + chartHeight - barHeight;

            var bar = new Rectangle
            {
                Width = Math.Max(barWidth, 1),
                Height = barHeight,
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)i, (byte)i, (byte)i)),
                Stroke = Brushes.DarkGray,
                StrokeThickness = 0.5,
            };

            HistogramCanvas.Children.Add(bar);
            Canvas.SetLeft(bar, x);
            Canvas.SetTop(bar, y);
        }
    }

    private void DrawAxes(double margin, double chartWidth, double chartHeight, int maxFrequency)
    {
        var yAxis = new Line
        {
            X1 = margin,
            Y1 = margin,
            X2 = margin,
            Y2 = margin + chartHeight,
            Stroke = Brushes.Black,
            StrokeThickness = 2,
        };
        HistogramCanvas.Children.Add(yAxis);

        var xAxis = new Line
        {
            X1 = margin,
            Y1 = margin + chartHeight,
            X2 = margin + chartWidth,
            Y2 = margin + chartHeight,
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };
        HistogramCanvas.Children.Add(xAxis);

        var xLabels = new[] { 0, 64, 128, 192, 255 };
        foreach (var label in xLabels)
        {
            var x = margin + (label / 255.0) * chartWidth;
            
            var text = new TextBlock
            {
                Text = label.ToString(),
                FontSize = 11,
                Foreground = Brushes.Black,
            };

            HistogramCanvas.Children.Add(text);
            Canvas.SetLeft(text, x - 10);
            Canvas.SetTop(text, margin + chartHeight + 5);

            var tick = new Line
            {
                X1 = x,
                Y1 = margin + chartHeight,
                X2 = x,
                Y2 = margin + chartHeight + 5,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            HistogramCanvas.Children.Add(tick);
        }
        
        const int yLabelCount = 5;
        for (var i = 0; i <= yLabelCount; i++)
        {
            double y = margin + chartHeight - (i / (double)yLabelCount) * chartHeight;
            int frequency = (int)(maxFrequency * i / (double)yLabelCount);

            TextBlock text = new TextBlock
            {
                Text = frequency.ToString(),
                FontSize = 11,
                Foreground = Brushes.Black
            };
            HistogramCanvas.Children.Add(text);
            Canvas.SetLeft(text, margin - 35);
            Canvas.SetTop(text, y - 8);

            // Znacznik na osi
            var tick = new Line
            {
                X1 = margin - 5,
                Y1 = y,
                X2 = margin,
                Y2 = y,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            HistogramCanvas.Children.Add(tick);
        }

        // Opisy osi
        var xLabel = new TextBlock
        {
            Text = "Wartość piksela (0-255)",
            FontSize = 12,
            FontWeight = FontWeights.Bold
        };
        HistogramCanvas.Children.Add(xLabel);
        Canvas.SetLeft(xLabel, margin + chartWidth / 2 - 80);
        Canvas.SetTop(xLabel, margin + chartHeight + 25);

        var yLabel = new TextBlock
        {
            Text = "Częstość",
            FontSize = 12,
            FontWeight = FontWeights.Bold,
            RenderTransform = new RotateTransform(-90)
        };
        HistogramCanvas.Children.Add(yLabel);
        Canvas.SetLeft(yLabel, 15);
        Canvas.SetTop(yLabel, margin + chartHeight / 2 + 30);
    }

    private void StretchHistogram(object sender, RoutedEventArgs e)
    {
        var recodingTable = HistogramStretch.CalculateStretch(_histogramData.Frequencies);
        var bitmap = new Bitmap(_parentWindow.Image.Width, _parentWindow.Image.Height, PixelFormat.Format8bppIndexed);
        
        var palette = bitmap.Palette;
        for (var i = 0; i < 256; i++)
        {
            palette.Entries[i] = Color.FromArgb(i, i, i);
        }
        bitmap.Palette = palette;
        
        var bitmapData = bitmap.LockBits(
            new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            PixelFormat.Format8bppIndexed);
        
        try
        {
            unsafe
            {
                var ptr = (byte*)bitmapData.Scan0;
                
                for (var y = 0; y < bitmap.Height; y++)
                {
                    for (var x = 0; x < bitmap.Width; x++)
                    {
                        var currentPixelValue = _parentWindow.Image.GetPixel(x, y).B;
                        var newPixelValue = recodingTable[currentPixelValue];
                        ptr[y * bitmapData.Stride + x] = newPixelValue;
                    }
                }
            }
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }

        var manager = WindowManager.GetInstance();
        var window = manager.AddWindow($"{_parentWindow.Title} stretched", bitmap);
        var imageWindow = new ImageWindow(window);
        imageWindow.Show();
    }

    private void StretchHistogramWithOversaturation(object sender, RoutedEventArgs e)
    {
        var recodingTable = HistogramStretch.CalculateStretchWithSaturation(_histogramData.Frequencies);
        
        var bitmap = _parentWindow.Image.Recode(recodingTable);

        var manager = WindowManager.GetInstance();
        var window = manager.AddWindow($"{_parentWindow.Title} stretched", bitmap);
        var imageWindow = new ImageWindow(window);
        imageWindow.Show();
    }

    private void Equalize(object sender, RoutedEventArgs e)
    {
        var recodingTable = HistogramStretch.Equalize(_histogramData.Frequencies);
        
        var bitmap = _parentWindow.Image.Recode(recodingTable);

        var manager = WindowManager.GetInstance();
        var window = manager.AddWindow($"{_parentWindow.Title} stretched", bitmap);
        var imageWindow = new ImageWindow(window);
        imageWindow.Show();
    }

    private void BinaryThreshold(object sender, RoutedEventArgs e)
    {
        
    }
}