using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Point;

namespace Presentation.Point;

public partial class DoubleThreshold : Window
{
    public Bitmap Image { get; private set; }
    public DoubleThreshold(Bitmap image)
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

    private void Calculate(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(Threshold1.Text, out var t1) && int.TryParse(Threshold2.Text, out var t2))
        {
            var recodingTable = PointOperations.DoubleThreshold(t1, t2);
            Image = Image.Recode(recodingTable);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter valid values for thresholds.", "Invalid Input");
        }
    }
}