using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Point;

namespace Presentation.Point;

public partial class GrayscaleThreshold : Window
{
    public GrayscaleThreshold(Bitmap bitmap)
    {
        InitializeComponent();
        Image = bitmap;
    }

    public Bitmap Image { get; private set; }

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
    
    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            var processedBitmap = PointOperations.GrayscaleThreshold(Image, (byte)numericValue);
            Image = processedBitmap;
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }
}