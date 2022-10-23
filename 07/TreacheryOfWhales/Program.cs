using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace TreacheryOfWhales
{
    class Program
    {
        static void Main(string[] args)
        {

            var inputData = DataLoader.LoadSingleLineInputData(int.Parse, ",");

            foreach (var inputSet in inputData)
            {
                if (!inputSet.Content.Any())
                {
                    continue;
                }

                Console.WriteLine($"> Part-1 for {inputSet.Name}");

                var value = Part1(inputSet);
                Console.WriteLine("Min Fuel: {0}", value);

                Console.WriteLine($"< Part-1 for {inputSet.Name}");


                Console.WriteLine($"> Part-2 for {inputSet.Name}");

                var value2 = Part2(inputSet);
                Console.WriteLine("Min Fuel: {0}", value2);

                Console.WriteLine($"< Part-2 for {inputSet.Name}");
            }
        }

        private static int Part1(PuzzleInput<int> input)
        {

            var max = input.Content.Max();
            var min = input.Content.Min();
            var dists = new List<int>();
            for (var i = min; i < max; i++)
            {
                var dist = 0;
                foreach (var crabSub in input.Content)
                {
                    dist += Math.Abs(i - crabSub);
                }
                dists.Add(dist);
            }
            return dists.Min();
        }

        private static int Part2(PuzzleInput<int> input)
        {
            var max = input.Content.Max();
            var min = input.Content.Min();
            var gausSum = new int[max+1];
            var dists = new List<int>();

            for (var i = 0; i <= max; i++)
            {
                gausSum[i] = ((i * i) + i) / 2;
            }

            for (var i = min; i < max; i++)
            {
                var distSum = 0;
                foreach (var crabSub in input.Content)
                {
                    var dist = Math.Abs(i - crabSub);
                    distSum += gausSum[dist];
                }
                dists.Add(distSum);
            }
            return dists.Min();
        }
    }
}
