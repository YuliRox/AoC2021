namespace src;

public readonly record struct Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; init; }
    public int Y { get; init; }

    public static Point operator +(Point point, Gradient gradient)
    {
        return new Point(point.X + gradient.DirectionX, point.Y + gradient.DirectionY);
    }
}