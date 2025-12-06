using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Blur;

namespace Presentation.Neighbour;

public partial class MedianBlurWindow : Window
{
    public Bitmap Image { get; private set; }
    public MedianBlurWindow(Bitmap image)
    {
        InitializeComponent();
        Image = image;
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

    private void CalculateMedianBlur(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue) && numericValue % 2 == 1)
        {
            Image = NeighbourOperations.MedianBlur(Image, numericValue);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter an odd number", "Invalid Input");
        }
    }
}