using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Blur;
using OpenCvSharp;
using Presentation.WindowManagement;
using Window = System.Windows.Window;

namespace Presentation.Neighbour;

public partial class BlurOperationsWindow : Window
{
    public Bitmap Image { get; private set; }
    public BlurOperationsWindow(ManagedWindow window)
    {
        InitializeComponent();
        Image = window.Image;
        BorderType.ItemsSource = typeof(BorderRuleTypes).GetEnumValues();
        BorderType.SelectedItem = BorderRuleTypes.BorderConstant;
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

    private void BlurMask1(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            Image = NeighbourOperations.Blur(Image, (BorderRuleTypes)BorderType.SelectedItem, numericValue, 0);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }
    
    private void BlurMask2(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            Image = NeighbourOperations.Blur(Image, (BorderRuleTypes)BorderType.SelectedItem, numericValue, 1);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }
}