namespace Presentation.Point.Math;
public class WindowViewmodel
{
    public required string Title { get; init; }
    public int Index { get; set; }
    public required Guid Id { get; set; }

    public override string ToString()
    {
        return Title;
    }
}