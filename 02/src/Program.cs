using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace src
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputData = DataLoader.LoadInputData<DirectionPuzzle>(TransformInputLine);
            foreach (var inputSet in inputData)
            {
                Console.WriteLine("-->{0}", inputSet.Name);


                var position = Part1(inputSet);
                Console.WriteLine("=== Part 1 ===");
                Console.WriteLine($"The horizontal position is: {position.horizontal}");
                Console.WriteLine($"The depth is: {position.vertical}");
                Console.WriteLine($"Total distance: {position.horizontal * position.vertical}");

                position = Part2(inputSet);
                Console.WriteLine("=== Part 2 ===");
                Console.WriteLine($"The horizontal position is: {position.horizontal}");
                Console.WriteLine($"The depth is: {position.vertical}");
                Console.WriteLine($"Total distance: {position.horizontal * position.vertical}");


                Console.WriteLine("<--{0}", inputSet.Name);
            }

        }

        private static (int horizontal, int vertical) Part1(PuzzleInput<DirectionPuzzle> input)
        {
            var forwardSum = input.Content
                .Where(x => x.Direction == Direction.FORWARD)
                .Sum(x => x.Amount);

            var depthSum = input.Content
                .Where(x => x.Direction == Direction.UP || x.Direction == Direction.DOWN)
                .Sum(x => x.Amount);

            return (forwardSum, depthSum);

        }

        private static (int horizontal, int vertical) Part2(PuzzleInput<DirectionPuzzle> input)
        {
            var aim = 0;
            var depth = 0;
            var horizontalPosition = 0;

            foreach (var movement in input.Content)
            {
                switch (movement.Direction)
                {
                    case Direction.UP:
                    case Direction.DOWN:
                        aim += movement.Amount;
                        break;
                    case Direction.FORWARD:
                        horizontalPosition += movement.Amount;
                        depth += aim * movement.Amount;
                        break;
                }
            }
            return (horizontalPosition, depth);
        }

        private static DirectionPuzzle TransformInputLine(string line)
        {
            var parts = line.Split(' ');
            var direction = (Direction)Enum.Parse(typeof(Direction), parts[0], true);
            var amount = int.Parse(parts[1]);

            if (direction == Direction.UP)
                amount *= -1;

            return new DirectionPuzzle(direction, amount);
        }
    }

    internal readonly struct DirectionPuzzle
    {
        public Direction Direction { get; init; }
        public int Amount { get; init; }

        public DirectionPuzzle(Direction direction, int amount)
        {
            Direction = direction;
            Amount = amount;
        }
    }

    internal enum Direction
    {
        UP = 1,
        DOWN = 2,
        FORWARD = 3,
    }
}
