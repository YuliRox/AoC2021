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

            var inputData = DataLoader.LoadInputData(TranformInputData);

            foreach (var inputSet in inputData)
            {
                Console.WriteLine($"> Part-1 for {inputSet.Name}");

                var rates = Part1(inputSet);

                Console.WriteLine("Gamma Rate: {0}", rates.gamma);
                Console.WriteLine("Epsilon Rate: {0}", rates.epsilon);
                Console.WriteLine("Power Consumption: {0}", rates.gamma * rates.epsilon);

                Console.WriteLine($"< Part-1 for {inputSet.Name}");


                Console.WriteLine($"> Part-2 for {inputSet.Name}");

                var value2 = Part2(inputSet);

                Console.WriteLine($"< Part-2 for {inputSet.Name}");

            }

        }

        private static int[] TranformInputData(string line)
        {
            return line.Select(input => (input == '1') ? 1 : 0).ToArray();
        }

        private static (int gamma, int epsilon) Part1(PuzzleInput<int[]> input)
        {
            var lineLength = input.Content.FirstOrDefault()?.Length ?? 0;
            if (lineLength == 0)
                return (0, 0);
            var rowCount = input.Content.Count;

            var rateConversion = Enumerable.Range(0, lineLength)
                      .Select(colIdx =>
                            input.Content.Select(line => line[colIdx]).Sum() > rowCount/2 ? 1 : 0
                      )
                      .ToArray();

            var gammaRateStr = String.Join(null, rateConversion);
            var gammaRate = Convert.ToInt32(gammaRateStr, 2);
            var epsilonRateStr = String.Join(null, rateConversion.Select(pos => pos == 1 ? 0 : 1));
            var epsilonRate = Convert.ToInt32(epsilonRateStr, 2);
            return (gammaRate, epsilonRate);
        }

        private static int Part2(PuzzleInput<int[]> input)
        {
            return 0;
        }
    }
}
