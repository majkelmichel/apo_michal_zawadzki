using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Point;

namespace Presentation.Point.Math;

public partial class NumberOperations : Window
{
    public Bitmap Image { get; private set; }

    public NumberOperations(Bitmap image)
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
    
    private void AddClick(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            var maxValue = Image.GetMaxPixelValue();
            var saturate = Saturate.IsChecked == true;
            var recodingTable = MathOperations.Number.Add((byte)numericValue, saturate, maxValue);
            Image = Image.Recode(recodingTable);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }
    
    private void MultiplyClick(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            var saturate = Saturate.IsChecked == true;
            var recodingTable = MathOperations.Number.Multiply((byte)numericValue, saturate);
            Image = Image.Recode(recodingTable);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }
    
    
    private void DivideClick(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            var recodingTable = MathOperations.Number.Divide((byte)numericValue);
            Image = Image.Recode(recodingTable);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }
}