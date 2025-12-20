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


        var pointsEnumerable = contours.ToList();
        foreach (var contour in pointsEnumerable)
        {
            Area.Text = ContourOperations.CalculateArea(contour).ToString("#.###");
            Perimeter.Text = ContourOperations.CalculatePerimeter(contour).ToString("#.###");
            AspectRatio.Text = ContourOperations.CalculateAspectRatio(contour).ToString("#.###");
            Extent.Text = ContourOperations.CalculateExtent(contour).ToString("#.###");
            Solidity.Text = ContourOperations.CalculateSolidity(contour).ToString("#.###");
            EquivalentDiameter.Text = ContourOperations.CalculateEquivalentDiameter(contour).ToString("#.###");
        }

        SaveToFile(pointsEnumerable);
    }

    private void SaveToFile(IEnumerable<OpenCvSharp.Point[]> contours)
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
        sw.WriteLine("Area;Perimeter;Aspect Ratio;Extent;Solidity;Equivalent Diameter;M00;M10;M01;M20;M11;M02;M30;M21;M12;M03;Mu20;Mu11;Mu02;Mu30;Mu21;Mu12;Mu03;Nu20;Nu11;Nu02;Nu30;Nu21;Nu12;Nu03");

        foreach (var contour in contours)
        {
            var m = ContourOperations.CalculateMoments(contour);
            var area = ContourOperations.CalculateArea(contour).ToString("#.###");
            var perimeter = ContourOperations.CalculatePerimeter(contour).ToString("#.###");
            var aspectRatio = ContourOperations.CalculateAspectRatio(contour).ToString("#.###");
            var extent = ContourOperations.CalculateExtent(contour).ToString("#.###");
            var solidity = ContourOperations.CalculateSolidity(contour).ToString("#.###");
            var equivalentDiameter = ContourOperations.CalculateEquivalentDiameter(contour).ToString("#.###");
            sw.Write($"{area};{perimeter};{aspectRatio};{extent};{solidity};{equivalentDiameter};");
            sw.Write($"{m.M00};{m.M10};{m.M01};{m.M20};{m.M11};{m.M02};{m.M30};{m.M21};{m.M12};{m.M03};{m.Mu20};{m.Mu11};{m.Mu02};{m.Mu30};{m.Mu21};{m.Mu12};{m.Mu03};{m.Nu20};{m.Nu11};{m.Nu02};{m.Nu30};{m.Nu21};{m.Nu12};{m.Nu03}");
            sw.WriteLine();
        }
    }

}