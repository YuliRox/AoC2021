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
            }
        }

        private static int Part1(List<int> currentList)
        {
            var successorList = (new[] { currentList.First() }).Concat(currentList)
                                                          .Take(currentList.Count);
            return currentList.Zip(successorList, (current, successor) => successor < current ? 1 : 0)
                                           .Sum();
        }
    }
}
