using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Shared
{
    public static class DataLoader
    {

        public static List<PuzzleInput<T>> LoadInputData<T>(Func<string, T> contentModificationFunction)
        {
            return Directory.GetFiles(@"./input", "*.txt")
                .Select(filePath => new FileInfo(filePath))
                .Where(fileInfo => fileInfo.Name.Contains('_'))
                .Select(fileInfo => new PuzzleInput<T>(
                    Path.GetFileNameWithoutExtension(fileInfo.Name)
                        .Split('_')
                        .Last(),
                    File.ReadAllLines(fileInfo.FullName)
                        .Select(contentModificationFunction)
                        .ToList()))
                .ToList();
        }

        public static List<PuzzleInput<string>> LoadInputData()
        {
            return LoadInputData<string>(line => line);
        }
    }

    public readonly struct PuzzleInput<T>
    {
        public readonly string Name { get; }
        public List<T> Content { get; }

        public PuzzleInput(string name, List<T> content)
        {
            Name = name;
            Content = content;
        }

    }

}
