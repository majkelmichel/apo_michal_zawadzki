using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Algorithms.Blur;
using OpenCvSharp;
using Presentation.WindowManagement;
using Window = System.Windows.Window;

namespace Presentation.Neighbour;

public partial class NeighbourOperationsWindow : Window
{
    private ManagedWindow _window;
    public Bitmap Image { get; private set; }
    public NeighbourOperationsWindow(ManagedWindow window)
    {
        InitializeComponent();
        _window = window;
        Image = _window.Image;
        BorderType.ItemsSource = typeof(BorderTypes).GetEnumValues();
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

    private void Blur(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(NumericInput.Text, out var numericValue))
        {
            Image = BorderType.SelectedItem != null ? BlurOperations.Blur(Image, (BorderTypes)BorderType.SelectedItem, numericValue) : BlurOperations.Blur(Image, borderWidth: numericValue);
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid numeric value.", "Invalid Input");
        }
    }
}