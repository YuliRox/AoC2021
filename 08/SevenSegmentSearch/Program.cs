using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace SevenSegmentSearch
{
    class Program
    {
        private static readonly SevenSegmentNumber[] validSegmentNumbers = new SevenSegmentNumber[]
            {
                new (){ NumberValue = 0, ActiveSegments = "abcefg" },
                new (){ NumberValue = 1, ActiveSegments = "cf" },
                new (){ NumberValue = 2, ActiveSegments = "acdeg" },
                new (){ NumberValue = 3, ActiveSegments = "acdfg" },
                new (){ NumberValue = 4, ActiveSegments = "bcdf" },
                new (){ NumberValue = 5, ActiveSegments = "abdfg" },
                new (){ NumberValue = 6, ActiveSegments = "abdefg" },
                new (){ NumberValue = 7, ActiveSegments = "acf" },
                new (){ NumberValue = 8, ActiveSegments = "abcdefg" },
                new (){ NumberValue = 9, ActiveSegments = "abcdfg" }
            };
        private static readonly Dictionary<string, SevenSegmentNumber> segmentTranslation =
            validSegmentNumbers.ToDictionary(x => x.ActiveSegments);

        static void Main(string[] args)
        {

            var inputData = DataLoader.LoadInputData(TransformInput);

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
                UniqueSignalPatterns = numberSets.Take(10).Select(segment => new SevenSegmentNumber() { NumberValue = CountSegments(segment), ActiveSegments = SortSegments(segment) }).ToArray(),
                FourDigitOutputValue = numberSets.Skip(10).Select(segment => new SevenSegmentNumber() { NumberValue = CountSegments(segment), ActiveSegments = SortSegments(segment) }).ToArray(),
            };

            return note;
        }

        private static string SortSegments(string segment)
        {
            return new string(segment.OrderBy(x => x).ToArray());
        }

        private static int? CountSegments(string segment)
        {
            return segment.Length switch
            {
                2 => 1,
                3 => 7,
                4 => 4,
                7 => 8,
                _ => null,
            };
        }

        public record SevenSegmentNumber
        {
            public int? NumberValue { get; set; }
            public string ActiveSegments { get; init; }

            public static char operator -(SevenSegmentNumber a, SevenSegmentNumber b)
            {
                return a.ActiveSegments.Except(b.ActiveSegments).Single();
            }

            public static bool Contains(SevenSegmentNumber a, SevenSegmentNumber b)
            {
                foreach (var left in b.ActiveSegments)
                {
                    if (!a.ActiveSegments.Contains(left))
                        return false;
                }

                return true;
            }
        }

        public record NumberMask
        {
            public char? Top { get; set; }
            public char? Middle { get; set; }
            public char? Bottom { get; set; }
            public char? LeftTop { get; set; }
            public char? LeftBottom { get; set; }
            public char? RightTop { get; set; }
            public char? RightBottom { get; set; }
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
            return input.Content
                .Select(SolvePart2)
                .ToArray()
                .Sum();

            //Console.WriteLine(String.Join(' ', testinput.UniqueSignalPatterns.Where(number => number.ActiveSegments.Length == 5).Select(ssn => ssn.ActiveSegments)));
            //Console.WriteLine(String.Join(' ', testinput.UniqueSignalPatterns.Where(number => number.ActiveSegments.Length == 6).Select(ssn => ssn.ActiveSegments)));
        }

        private static int SolvePart2(NoteEntry note)
        {
            var digitOne = note.UniqueSignalPatterns.Single(x => x.NumberValue == 1);
            var digitFour = note.UniqueSignalPatterns.Single(x => x.NumberValue == 4);
            var digitSeven = note.UniqueSignalPatterns.Single(x => x.NumberValue == 7);
            var digitEight = note.UniqueSignalPatterns.Single(x => x.NumberValue == 8);

            var digitThree = note.UniqueSignalPatterns
                .Where(x => x.ActiveSegments.Length == 5)
                .Where(x => SevenSegmentNumber.Contains(x, digitOne))
                .Single();
            digitThree.NumberValue = 3;

            var digitNine = note.UniqueSignalPatterns
                .Where(x => x.ActiveSegments.Length == 6)
                .Where(x => SevenSegmentNumber.Contains(x, digitThree))
                .Single();
            digitNine.NumberValue = 9;

            var mask = new NumberMask()
            {
                LeftTop = digitNine - digitThree
            };

            var digitTwo = note.UniqueSignalPatterns
                .Where(x => x.ActiveSegments.Length == 5)
                .Where(x => x.NumberValue == null)
                .Where(x => !x.ActiveSegments.Contains(mask.LeftTop.Value))
                .Single();
            digitTwo.NumberValue = 2;

            var digitFive = note.UniqueSignalPatterns
                .Where(x => x.ActiveSegments.Length == 5)
                .Where(x => x.ActiveSegments.Contains(mask.LeftTop.Value))
                .Single();
            digitFive.NumberValue = 5;

            var digitZero = note.UniqueSignalPatterns
                .Where(x => x.ActiveSegments.Length == 6)
                .Where(x => x.NumberValue == null)
                .Where(x => SevenSegmentNumber.Contains(x, digitOne))
                .Single();
            digitZero.NumberValue = 0;

            var digitSix = note.UniqueSignalPatterns
                .Where(x => x.NumberValue == null)
                .Single();
            digitSix.NumberValue = 6;


            mask.Top = digitSeven - digitOne;
            //mask.LeftTop = digitNine - digitThree;
            mask.RightTop = digitNine - digitFive;
            mask.Middle = digitEight - digitZero;
            mask.LeftBottom = digitSix - digitFive;
            mask.RightBottom = digitThree - digitTwo;
            mask.Bottom = digitEight.ActiveSegments.Except(new[] {
                mask.Top.Value,
                mask.LeftTop.Value,
                mask.RightTop.Value,
                mask.Middle.Value,
                mask.LeftBottom.Value,
                mask.RightBottom.Value
            }).Single();

            var translation = new Dictionary<char, char>()
            {
                /*{'a', mask.Top.Value },
                {'b', mask.LeftTop.Value },
                {'c', mask.RightTop.Value },
                {'d', mask.Middle.Value },
                {'e', mask.LeftBottom.Value },
                {'f', mask.RightBottom.Value },
                {'g', mask.Bottom.Value },*/

                {mask.Top.Value, 'a' },
                {mask.LeftTop.Value, 'b' },
                {mask.RightTop.Value, 'c' },
                {mask.Middle.Value, 'd' },
                {mask.LeftBottom.Value, 'e' },
                {mask.RightBottom.Value, 'f' },
                {mask.Bottom.Value, 'g' },
            };

            foreach (var outDigit in note.FourDigitOutputValue.Where(x => x.NumberValue == null))
            {
                var translatedRawDigit = SortSegments(new string(TranslateDigit(translation, outDigit.ActiveSegments).ToArray()));
                var replacement = segmentTranslation[translatedRawDigit];
                outDigit.NumberValue = replacement.NumberValue;
            }


            var outNumber = 0;
            for (int i = 0; i < note.FourDigitOutputValue.Length; i++)
            {
                outNumber += note.FourDigitOutputValue[i].NumberValue.Value * (int)Math.Pow(10, (note.FourDigitOutputValue.Length - 1 - i));
            }

            return outNumber;
        }

        private static IEnumerable<char> TranslateDigit(Dictionary<char, char> map, string input)
        {
            foreach (var inChar in input)
            {
                yield return map[inChar];
            }
        }
    }
}
