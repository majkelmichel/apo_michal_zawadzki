using System.Drawing;
using System.Windows;
using Algorithms.Morphology;

namespace Presentation.Neighbour;

public partial class MorphologyOperationsWindow : Window
{
    public Bitmap Image { get; private set; }
    public MorphologyOperationsWindow(Bitmap image)
    {
        Image = image;
        InitializeComponent();
        StructuringElementSelect.ItemsSource = typeof(StructuringElements).GetEnumValues();
        StructuringElementSelect.SelectedItem = StructuringElements.Rectangle;
    }

    private void Erode(object sender, RoutedEventArgs e)
    {
        Image = MorphologyOperations.Erode(Image, (StructuringElements)StructuringElementSelect.SelectedIndex);
        Close();
    }
    
    private void Dilate(object sender, RoutedEventArgs e)
    {
        Image = MorphologyOperations.Erode(Image, (StructuringElements)StructuringElementSelect.SelectedIndex);
        Close();
    }

    private void Open(object sender, RoutedEventArgs e)
    {
        Image = MorphologyOperations.Open(Image, (StructuringElements)StructuringElementSelect.SelectedIndex);
        Close();
    }
    
    private void Close(object sender, RoutedEventArgs e)
    {
        Image = MorphologyOperations.Close(Image, (StructuringElements)StructuringElementSelect.SelectedIndex);
        Close();
    }

    private void Skeletonize(object sender, RoutedEventArgs e)
    {
        Image = MorphologyOperations.Skeletonize(Image.ToGrayscale());
        Close();
    }
}