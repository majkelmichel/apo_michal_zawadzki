using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Blur;
using Presentation.WindowManagement;

namespace Presentation.Neighbour;

public partial class SharpenOperationsWindow : Window
{
    public Bitmap Image { get; private set; }
    public SharpenOperationsWindow(ManagedWindow window)
    {
        InitializeComponent();
        Image = window.Image;
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

    private void SharpenMask1(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            Image = NeighbourOperations.Sharpen(Image, (BorderRuleTypes)BorderType.SelectedItem, numericValue, 0);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }

    private void SharpenMask2(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            Image = NeighbourOperations.Sharpen(Image, (BorderRuleTypes)BorderType.SelectedItem, numericValue, 1);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }

    private void SharpenMask3(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            Image = NeighbourOperations.Sharpen(Image, (BorderRuleTypes)BorderType.SelectedItem, numericValue, 2);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }
}