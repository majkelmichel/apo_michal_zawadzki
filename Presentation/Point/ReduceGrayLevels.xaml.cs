using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Point;

namespace Presentation.Point;

public partial class ReduceGrayLevels : Window
{
    public Bitmap Image { get; private set; }

    public ReduceGrayLevels(Bitmap image)
    {
        InitializeComponent();
        Image = image;
    }

    // Only allow numeric input
    private void NumericInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Allow only digits
        e.Handled = !IsTextNumeric(e.Text);
    }

    // Prevent pasting non-numeric text
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

    private bool IsTextNumeric(string text)
    {
        return Numeric().IsMatch(text);
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            var processedBitmap = PointOperations.ReduceGrayLevels(Image, (byte)numericValue);
            Image = processedBitmap;
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }

    [GeneratedRegex("^[0-9]+$")]
    private static partial Regex Numeric();
}