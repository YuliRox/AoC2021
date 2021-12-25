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

                var lifeSupportRating = Part2(inputSet);
                Console.WriteLine("Life support rating: {0}", lifeSupportRating);

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
                            input.Content.Select(line => line[colIdx]).Sum() > rowCount / 2 ? 1 : 0
                      )
                      .ToArray();

            var gammaRateStr = String.Join(null, rateConversion);
            var gammaRate = Convert.ToInt32(gammaRateStr, 2);
            var epsilonRateStr = String.Join(null, rateConversion.Select(pos => pos == 1 ? 0 : 1));
            var epsilonRate = Convert.ToInt32(epsilonRateStr, 2);
            return (gammaRate, epsilonRate);
        }

        private static int BinaryToInteger(int[] binary)
        {
            if(binary == null)
                return 0;

            var binaryStr = string.Join(null, binary);
            return Convert.ToInt32(binaryStr, 2);
        }

        private static int Part2(PuzzleInput<int[]> input)
        {
            var oxRating = input.Content.ToArray();
            var coRating = input.Content.ToArray();

            var oxRatingRaw = CriteriaSieve(oxRating, (significance, totalAmount) => (significance >= totalAmount / 2.0));
            var coRatingRaw = CriteriaSieve(coRating, (significance, totalAmount) => (significance < totalAmount / 2.0));

            var oxRatingValue = BinaryToInteger(oxRatingRaw);
            var coRatingValue = BinaryToInteger(coRatingRaw);

            Console.WriteLine("Oxygen generator rating: {0}", oxRatingValue);
            Console.WriteLine("Co2 scrubber rating: {0}", coRatingValue);

            return oxRatingValue * coRatingValue;
        }

        private static int[] CriteriaSieve(int[][] input, Func<double, double, bool> useOnes)
        {

            var maximumColumnCount = input.FirstOrDefault()?.Length ?? 0;
            for (int currentBitPosition = 0; currentBitPosition < maximumColumnCount; currentBitPosition++)
            {
                var significance = input.Select(row => row[currentBitPosition]).Sum();
                var inputLength = input.Length;
                var preference = useOnes(significance, inputLength);
                input = input.Where(row =>
                    preference ?
                        row[currentBitPosition] == 1 :
                        row[currentBitPosition] == 0
                ).ToArray();

                if(input.Length == 1) {
                    return input.Single();
                }
            }
            return default;
        }
    }
}
