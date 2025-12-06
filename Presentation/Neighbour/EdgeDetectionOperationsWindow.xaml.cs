using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Blur;

namespace Presentation.Neighbour;

public partial class EdgeDetectionOperationsWindow : Window
{
    public Bitmap Image { get; private set; }
    
    public EdgeDetectionOperationsWindow(Bitmap image) 
    {
        Image = image;
        InitializeComponent();
        BorderType.ItemsSource = typeof(BorderRuleTypes).GetEnumValues();
    }
    
    [GeneratedRegex("^[0-9]+$")]
    private static partial Regex Numeric();
    
    private void NumericInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !IsTextNumeric(e.Text);
    }

    private bool IsTextNumeric(string text)
    {
        return Numeric().IsMatch(text);
    }

    private void NumericInput_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            string text = (string)e.DataObject.GetData(typeof(string));
            if (!IsTextNumeric(text))
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }

    private void EdgeDetection(EdgeDetectionDirection direction)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            if (BorderType.SelectedItem is not BorderRuleTypes borderRuleTypes) borderRuleTypes = BorderRuleTypes.BorderConstant;
            Image = NeighbourOperations.PrewittEdge(Image, borderRuleTypes, numericValue, direction);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }

    private void EdgeDetectionN(object sender, RoutedEventArgs e)
    {
        EdgeDetection(EdgeDetectionDirection.N);
    }

    private void EdgeDetectionNE(object sender, RoutedEventArgs e)
    {
        EdgeDetection(EdgeDetectionDirection.NE);
    }
    
    private void EdgeDetectionE(object sender, RoutedEventArgs e)
    {
        EdgeDetection(EdgeDetectionDirection.E);
    }
    
    private void EdgeDetectionSE(object sender, RoutedEventArgs e)
    {
        EdgeDetection(EdgeDetectionDirection.SE);
    }
    
    private void EdgeDetectionS(object sender, RoutedEventArgs e)
    {
        EdgeDetection(EdgeDetectionDirection.S);
    }
    
    private void EdgeDetectionSW(object sender, RoutedEventArgs e)
    {
        EdgeDetection(EdgeDetectionDirection.SW);
    }
    
    private void EdgeDetectionW(object sender, RoutedEventArgs e)
    {
        EdgeDetection(EdgeDetectionDirection.W);
    }
    
    private void EdgeDetectionNW(object sender, RoutedEventArgs e)
    {
        EdgeDetection(EdgeDetectionDirection.NW);
    }
}