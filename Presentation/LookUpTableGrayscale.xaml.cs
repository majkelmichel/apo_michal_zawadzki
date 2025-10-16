using System.Windows;

namespace Presentation;

public partial class LookUpTableGrayscale : Window
{
    public IEnumerable<LutRowViewModel> DisplayRows { get; set; }
    
    public LookUpTableGrayscale(Algorithms.LookUpTable.LookUpTableGrayscale lut)
    {
        DisplayRows = lut.Rows.Select((row, index) => new LutRowViewModel
        {
            Index = (byte)index,
            Intensity = row.Intensity
        });
        InitializeComponent();
        DataContext = this;
        Owner = Application.Current.MainWindow;
    }
    
    public class LutRowViewModel
    {
        public byte Index { get; set; }
        public byte Intensity { get; set; }
    }
}