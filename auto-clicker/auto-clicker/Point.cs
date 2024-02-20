namespace auto_clicker;

public struct Point(int x, int y)
{
    public int X { get; private set; } = x;

    public int Y { get; private set; } = y;

    public override readonly string ToString() => $"x:{X}, y:{Y}";
}
