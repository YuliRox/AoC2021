using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace src
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputData = Directory.GetFiles(@"./input", "*.txt")
                .Select(filePath => new FileInfo(filePath))
                .Where(fileInfo => fileInfo.Name.Contains('_'))
                .Select(fileInfo => new
                {
                    Name = Path.GetFileNameWithoutExtension(fileInfo.Name)
                        .Split('_')
                        .Last(),
                    Content = File.ReadAllLines(fileInfo.FullName)
                        .Select(int.Parse)
                        .ToList()
                });

            foreach (var input in inputData)
            {
                Console.WriteLine($"> Part-1 for {input.Name}");

                var depthIncrease = Part1(input.Content);
                Console.WriteLine($"Depth increased a total of {depthIncrease} times");

                Console.WriteLine($"< Part-1 for {input.Name}");


                Console.WriteLine($"> Part-2 for {input.Name}");

                var depthIncrease2 = Part2(input.Content);
                Console.WriteLine($"Depth increased {depthIncrease2} times in a window of 3");

                Console.WriteLine($"< Part-2 for {input.Name}");
            }
        }

        private static int Part1(List<int> currentList)
        {
            var successorList = (new[] { currentList.First() }).Concat(currentList)
                                                          .Take(currentList.Count);
            return currentList.Zip(successorList, (current, successor) => successor < current ? 1 : 0)
                                           .Sum();
        }

        private static int Part2(List<int> inputData)
        {
            var windowedData = inputData
                // append index to values
                .Zip(Enumerable.Range(0, inputData.Count),
                    (value, index) =>
                        new { Index = index, Value = value })
                // select sub-lists of data
                .Select(pair => inputData
                    .Skip(pair.Index)
                    .Take(3) // with a length of maximum 3
                    .ToArray())
                // filter sets with less than 3 elements
                .Where(result => result.Length == 3)
                // sum their result
                .Select(result => result.Sum())
                .ToList();

            return Part1(windowedData);
        }
    }
}
