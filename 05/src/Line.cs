using System;
using System.Drawing;

namespace src;
public readonly struct Line
{
    public Line(int x1, int y1, int x2, int y2)
    {
        Start = new Point(x1, y1);
        End = new Point(x2, y2);
        Gradient = new Gradient(Start, End).Normalize();
    }

    public Point Start { get; init; }
    public Point End { get; init; }

    public Gradient Gradient { get; init; }
}

public readonly struct Gradient
{
    public int DirectionX { get; init; }
    public int DirectionY { get; init; }
    public Gradient(Point start, Point end) : this(end.X - start.X, end.Y - start.Y)
    {
    }

    public Gradient(int directionX, int directionY)
    {
        DirectionX = directionX;
        DirectionY = directionY;
    }

    public Gradient Normalize()
    {
        // TODO fix for angles not 45°
        return new Gradient(Math.Sign(DirectionX), Math.Sign(DirectionY));
    }

    public bool IsUniform { get => !(DirectionX != 0 && DirectionY != 0); }
}