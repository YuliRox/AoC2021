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

        }
    }
}
