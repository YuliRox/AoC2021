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

            var inputData = DataLoader.LoadInputData(line => line.Split(',').Select(int.Parse).ToList());

            foreach (var inputSet in inputData)
            {
                if (!inputSet.Content.Any())
                {
                    continue;
                }

                Console.WriteLine($"> Part-1 for {inputSet.Name}");

                var value = Part1(inputSet);
                Console.WriteLine($"Number of fishies: {value} 🐟");

                Console.WriteLine($"< Part-1 for {inputSet.Name}");


                Console.WriteLine($"> Part-2 for {inputSet.Name}");

                var value2 = Part2(inputSet);
                Console.WriteLine($"Number of fishies: {value2} 🐟");

                Console.WriteLine($"< Part-2 for {inputSet.Name}");
            }
        }

        private static int Part1(PuzzleInput<List<int>> input)
        {
            var fishSchool = input.Content.Single().ToList();
            var days = 80;

            for (int i = 0; i < days; i++)
            {
                var fishies = fishSchool.Count;
                for (int fishyNumber = 0; fishyNumber < fishies; fishyNumber++)
                {
                    if (fishSchool[fishyNumber] != 0)
                    {
                        fishSchool[fishyNumber]--;
                    }
                    else
                    {
                        fishSchool[fishyNumber] = 6;
                        fishSchool.Add(8);
                    }
                }
                //Console.WriteLine($"After {i + 1} days: {String.Join(',', fishSchool)}");
            }

            return fishSchool.Count;
        }

        private static ulong Part2(PuzzleInput<List<int>> input)
        {
            var fishSchool = input.Content.Single();
            var fishQueue = new List<ulong>();

            for(var fishyNumber = 0; fishyNumber <= 8; fishyNumber++) {
                fishQueue.Add((ulong)fishSchool.Count(fishy => fishy == fishyNumber));
            }

            var days = 256;

            for (var i = 0; i < days; i++)
            {
                var newFishies = fishQueue.First();
                fishQueue.RemoveAt(0);
                fishQueue.Add(0);

                fishQueue[6] += newFishies;
                fishQueue[8] += newFishies;
            }

            ulong bigSum = 0;
            foreach(var numberOfFishies in fishQueue) {
                bigSum += (ulong)numberOfFishies;
            }

            return bigSum;
        }
    }
}
