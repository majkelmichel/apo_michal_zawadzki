namespace Algorithms.Histogram;

public class HistogramData
{
    public int[] Frequencies { get; init; } = [];
    public double Mean { get; init; }
    public double StandardDeviation { get; init; }
    public double Median { get; init; }
    public int Max { get; init; }
    public int Min { get; init; }
    public int TotalPixels { get; init; }
}