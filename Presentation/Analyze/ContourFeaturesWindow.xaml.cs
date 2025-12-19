using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using Algorithms;
using Microsoft.Win32;
using OpenCvSharp;
using Window = System.Windows.Window;

namespace Presentation.Analyze;

public partial class ContourFeaturesWindow : Window
{
    public ContourFeaturesWindow(Bitmap image)
    {
        InitializeComponent();
        var contours = ContourOperations.CalculateContour(image);
        
        

        foreach (var contour in contours)
        {
            // var moments = ContourOperations.CalculateMoments(contour).ToString();
            Area.Text = ContourOperations.CalculateArea(contour).ToString("#.###");
            Perimeter.Text = ContourOperations.CalculatePerimeter(contour).ToString("#.###");
            AspectRatio.Text = ContourOperations.CalculateAspectRatio(contour).ToString("#.###");
            Extent.Text = ContourOperations.CalculateExtent(contour).ToString("#.###");
            Solidity.Text = ContourOperations.CalculateSolidity(contour).ToString("#.###");
            EquivalentDiameter.Text = ContourOperations.CalculateEquivalentDiameter(contour).ToString("#.###");
        }
    }

    private void SaveToFile()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save Results",
            Filter = "CSV File|*.csv",
            DefaultExt = ".csv",
            FileName = "ContourFeatures"
        };
        
        if (saveFileDialog.ShowDialog() != true) return;
        
        using var sw = new StreamWriter(saveFileDialog.FileName);
        sw.WriteLine("Area;Perimeter;Aspect Ratio;Extent;Solidity;Equivalent Diameter");

        sw.WriteLine($"{Area.Text};{Perimeter.Text};{AspectRatio.Text};{Extent.Text};{Solidity.Text};{EquivalentDiameter.Text}");
    }

    private void SaveButtonClick(object sender, RoutedEventArgs e)
    {
        SaveToFile();
    }
}