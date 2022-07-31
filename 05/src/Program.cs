using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace src;

class Program
{
    static void Main(string[] args)
    {

        var inputData = DataLoader.LoadInputData(ToLineOutput);

        foreach (var inputSet in inputData)
        {
            if (inputSet.Content.Length <= 0)
                continue;

            Console.WriteLine($"> Part-1 for {inputSet.Name}");

            var value = Part1(inputSet);
            Console.WriteLine($"Number of Dangerous Points: {value}");

            Console.WriteLine($"< Part-1 for {inputSet.Name}");


            Console.WriteLine($"> Part-2 for {inputSet.Name}");

            var value2 = Part2(inputSet);
            Console.WriteLine($"Number of Dangerous Points: {value2}");

            Console.WriteLine($"< Part-2 for {inputSet.Name}");
        }
    }

    private static Line ToLineOutput(string arg)
    {
        var rawCoords = arg.Replace("->", ",").Split(',').Select(int.Parse).ToArray();
        return new Line(rawCoords[0], rawCoords[1], rawCoords[2], rawCoords[3]);
    }

    private static int Part1(PuzzleInput<Line> input)
    {
        var maxX = input.Content.Max(line => Math.Max(line.Start.X, line.End.X));
        var maxY = input.Content.Max(line => Math.Max(line.Start.Y, line.End.Y));

        var solution = new SolutionBoard(maxX, maxY);
        //skip 45° lines
        foreach (var line in input.Content.Where(line => line.Gradient.IsUniform))
        {
            solution.WriteLineToBoard(line);
        }

        return solution.CountDangerousAreas();

    }

    private static int Part2(PuzzleInput<Line> input)
    {
        var maxX = input.Content.Max(line => Math.Max(line.Start.X, line.End.X));
        var maxY = input.Content.Max(line => Math.Max(line.Start.Y, line.End.Y));

        var solution = new SolutionBoard(maxX, maxY);
        //skip 45° lines
        foreach (var line in input.Content)
        {
            solution.WriteLineToBoard(line);
        }

        return solution.CountDangerousAreas();
    }
}