using System.Windows;

namespace Presentation.Analyze;

public partial class LookUpTableColor : Window
{
    public IEnumerable<LutRowViewModel> DisplayRows { get; set; }
    public LookUpTableColor(Algorithms.LookUpTable.LookUpTableColor lut)
    {
        DisplayRows = lut.Rows.Select((row, index) => new LutRowViewModel
        {
            Index = (byte)index,
            Red = row.Red,
            Green = row.Green,
            Blue = row.Blue
        });
        InitializeComponent();
        DataContext = this;
        Owner = Application.Current.MainWindow;
    }

    public class LutRowViewModel
    {
        public byte Index { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }
}