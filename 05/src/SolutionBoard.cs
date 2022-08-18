using System;
using System.Linq;

namespace src;

public class SolutionBoard
{
    public SolutionBoard(int maxX, int maxY)
    {
        Board = new int[maxX + 1][];
        for (var index = 0; index < maxX + 1; index++)
        {
            Board[index] = new int[maxY + 1];
        }
    }

    public int[][] Board { get; init; }

    public int CountDangerousAreas()
    {
        return Board.SelectMany(col => col).Where(point => point >= 2).Count();
    }

    public void WriteLineToBoard(Line line)
    {
        for (var currentPosition = line.Start; currentPosition != line.End + line.Gradient; currentPosition += line.Gradient)
        {
            Board[currentPosition.X][currentPosition.Y] += 1;
        }
    }

    public void RenderBoard()
    {
        for (var indexY = 0; indexY < Board[0].Length; indexY++)
        {
            for (var indexX = 0; indexX < Board.Length; indexX++)
            {
                var position = Board[indexX][indexY];
                Console.Write(position == 0 ? "." : position.ToString());
            }
            Console.WriteLine();
        }
    }
}
