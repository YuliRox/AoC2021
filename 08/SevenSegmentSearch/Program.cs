using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace SevenSegmentSearch
{
    class Program
    {
        static void Main(string[] args)
        {

            var inputData = DataLoader.LoadInputData(TransformInput);

            var validSegmentNumbers = new SevenSegmentNumber[]
            {
                new (){NumberValue = 0, ActiveSegments = "abcefg" },
                new (){NumberValue = 1, ActiveSegments = "cf" },
                new (){NumberValue = 2, ActiveSegments = "acdeg" },
                new (){NumberValue = 3, ActiveSegments = "acdfg" },
                new (){NumberValue = 4, ActiveSegments = "bcdf" },
                new (){NumberValue = 5, ActiveSegments = "abdfg" },
                new (){NumberValue = 6, ActiveSegments = "abdefg" },
                new (){NumberValue = 7, ActiveSegments = "acf" },
                new (){NumberValue = 8, ActiveSegments = "abcdefg" },
                new (){NumberValue = 9, ActiveSegments = "abcdfg" }
            };

            foreach (var inputSet in inputData)
            {
                if (!inputSet.Content.Any())
                {
                    continue;
                }
                Console.WriteLine($"> Part-1 for {inputSet.Name}");

                var value = Part1(inputSet);
                Console.WriteLine("Value: {0}", value);

                Console.WriteLine($"< Part-1 for {inputSet.Name}");


                Console.WriteLine($"> Part-2 for {inputSet.Name}");

                var value2 = Part2(inputSet);
                Console.WriteLine("Value: {0}", value2);

                Console.WriteLine($"< Part-2 for {inputSet.Name}");
            }
        }

        private static NoteEntry TransformInput(string line)
        {
            var numberSets = line.Split(" | ").SelectMany(element => element.Split(" "));

            var note = new NoteEntry()
            {
                UniqueSignalPatterns = numberSets.Take(10).Select(segment => new SevenSegmentNumber() { NumberValue = CountSegments(segment), ActiveSegments = segment }).ToArray(),
                FourDigitOutputValue = numberSets.Skip(10).Select(segment => new SevenSegmentNumber() { NumberValue = CountSegments(segment), ActiveSegments = segment }).ToArray(),
            };

            return note;
        }

        private static int? CountSegments(string segment)
        {
           switch (segment.Length)
            {
                case 2:
                    return 1;
                case 3:
                    return 7;
                case 4:
                    return 4;
                case 7:
                    return 8;
                default:
                    return null;
            }
                
        }

        public record SevenSegmentNumber
        {
            public int? NumberValue { get; set; }
            public string ActiveSegments { get; init; }
        }

        public record NoteEntry
        {
            public SevenSegmentNumber[] UniqueSignalPatterns { get; init; } = new SevenSegmentNumber[10];
            public SevenSegmentNumber[] FourDigitOutputValue { get; init; } = new SevenSegmentNumber[4];
        }

        private static int Part1(PuzzleInput<NoteEntry> input)
        {
            return input.Content.SelectMany(noteEntry => noteEntry.FourDigitOutputValue).Where(unique => unique.NumberValue != null).Count();
        }

        private static int Part2(PuzzleInput<NoteEntry> input)
        {
            return 0;
        }
    }
}
