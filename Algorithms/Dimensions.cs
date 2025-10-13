namespace Algorithms;

public class Dimensions
{
    private Stream stream;
    public int Width { get; }
    public int Height { get; }
    
    public Dimensions(Stream imageStream)
    {
        stream = imageStream;
    }

}