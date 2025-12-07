using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Presentation.Neighbour;

public partial class CannyEdgeDetectionWindow : Window
{
    public CannyEdgeDetectionWindow()
    {
        InitializeComponent();
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
        throw new NotImplementedException();
    }
}