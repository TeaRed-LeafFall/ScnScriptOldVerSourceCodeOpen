namespace ScnScript;

public struct Point2
{
    public int Start { get; set; } = -1;
    public int End { get; set; } = -1;
    public Point2(int start, int end)
    {
        Start = start;
        End = end;
    }
    public Point2(int start)
    {
        Start = start;
    }

}
