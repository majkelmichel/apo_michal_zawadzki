using System.Windows;

namespace Presentation;

public partial class LookUpTableGrayscale : Window
{
    public Algorithms.LookUpTableGrayscale Lut { get; set; }
    public IEnumerable<LutRowViewModel> DisplayRows { get; set; }
    
    public LookUpTableGrayscale(Algorithms.LookUpTableGrayscale lut)
    {
        Lut = lut;
        DisplayRows = lut.Rows.Select((row, index) => new LutRowViewModel
        {
            Index = (byte)index,
            Intensity = row.Intensity
        });
        InitializeComponent();
        DataContext = this;
    }
    
    public class LutRowViewModel
    {
        public byte Index { get; set; }
        public byte Intensity { get; set; }
    }
}