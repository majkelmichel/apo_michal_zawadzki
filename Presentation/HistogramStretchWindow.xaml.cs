using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Histogram;

namespace Presentation;

public partial class HistogramStretchWindow : Window
{
    public Bitmap Image { get; private set; }
    public HistogramStretchWindow(Bitmap image)
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

    private void StretchHistogram(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(p1.Text, out var p1val) &&
            int.TryParse(p2.Text, out var p2val) &&
            int.TryParse(q1.Text, out var q1val) &&
            int.TryParse(q2.Text, out var q2val))
        {
            var recodingTable = HistogramStretch.StretchByValues(p1val, p2val, q1val, q2val);
            Image = Image.Recode(recodingTable);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter valid values for p1, p2, q1 and q2.", "Invalid Input");
        }
    }
}