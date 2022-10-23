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

                Console.WriteLine($"< Part-1 for {inputSet.Name}");


                Console.WriteLine($"> Part-2 for {inputSet.Name}");

                var value2 = Part2(inputSet);

                Console.WriteLine($"< Part-2 for {inputSet.Name}");
            }
        }

        private static int Part1(PuzzleInput<int> input)
        {

            var max = input.Content.Max();
            var min = input.Content.Min();
            foreach (var crabSub in input.Content)
            {
                for(var i = 0; i < max; i++)
                {

                }
            }
            return 0;
        }

        private static int Part2(PuzzleInput<int> input)
        {
            return 0;
        }
    }
}
