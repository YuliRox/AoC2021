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

                var forwardSum = inputSet.Content
                    .Where(x => x.Direction == Direction.FORWARD)
                    .Sum(x => x.Amount);

                var depthSum = inputSet.Content
                    .Where(x => x.Direction == Direction.UP || x.Direction == Direction.DOWN)
                    .Sum(x => x.Amount);

                Console.WriteLine($"The horizontal position is: {forwardSum}");
                Console.WriteLine($"The depth is: {depthSum}");
                Console.WriteLine($"Total distance: {forwardSum * depthSum}");

                Console.WriteLine("<--{0}", inputSet.Name);
            }

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
