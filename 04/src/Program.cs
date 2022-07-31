using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace src;

class Program
{
    static void Main(string[] args)
    {

        var inputData = DataLoader.LoadInputData();
        var puzzleInput = inputData.Select(p => new BingoInput(p)).ToArray();

        foreach (var puzzle in puzzleInput)
        {
            Console.WriteLine($"> Part-1 for {puzzle.Name}");

            var value = Part1(puzzle);
            Console.WriteLine($"Final score is {value}");

            Console.WriteLine($"< Part-1 for {puzzle.Name}");


            Console.WriteLine($"> Part-2 for {puzzle.Name}");

            var value2 = Part2(puzzle);
            Console.WriteLine($"Final score is {value2}");

            Console.WriteLine($"< Part-2 for {puzzle.Name}");
        }
    }

    private static int Part1(BingoInput input)
    {
        // check first board
        foreach (var number in input.DrawNumbers)
        {
            foreach (var board in input.Boards)
            {
                board.MarkPosition(number);
                if (board.CheckWinCondition())
                {
                    return board.CalculateScore(number);
                }
            }
        }

        return -1;
    }

    private static int Part2(BingoInput input)
    {
        // check last board
        var openBoards = input.Boards;

        foreach (var number in input.DrawNumbers)
        {
            /*if (openBoards.Length <= 0)
                break;*/

            foreach (var board in openBoards)
            {
                board.MarkPosition(number);
            }

            var closedBoards = openBoards
                .Where(board => board.CheckWinCondition())
                .ToArray();

            var remainingBoards = openBoards
                .Where(board => !board.CheckWinCondition())
                .ToArray();

            openBoards = remainingBoards;

            if (remainingBoards.Length <= 0 && closedBoards.Length > 0)
            {
                return closedBoards.Max(board => board.CalculateScore(number));
            }
        }

        return -1;
    }
}

public record class BingoInput : PuzzleInput<string>
{
    public int[] DrawNumbers { get; init; }

    public BingoBoard[] Boards { get; init; }

    public BingoInput(PuzzleInput<string> original) : base(original)
    {
        Name = original.Name;
        var content = original.Content;

        if (content.Length <= 0)
        {
            DrawNumbers = Array.Empty<int>();
            return;
        }

        DrawNumbers = content[0]
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        var rawBoards = content
            .Skip(2)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        if (rawBoards.Length % 5 != 0) throw new InvalidDataException("Boardlength does not fit expected format");

        Boards = new BingoBoard[rawBoards.Length / 5];

        for (var lineCount = 0; lineCount < rawBoards.Length; lineCount += 5)
        {
            var rawBoardStrings = rawBoards[lineCount..(lineCount + 5)];
            var rawBoardString = string.Join(' ', rawBoardStrings);
            var rawBoard = rawBoardString
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Select(x => new BingoPosition(x, false))
                .ToArray();

            Boards[lineCount / 5] = new BingoBoard(rawBoard);
        }
    }
}

public readonly record struct BingoBoard
{
    public int BoardSize { get; init; }
    public BingoBoard(BingoPosition[] board)
    {
        Board = board;
        BoardSize = (int)Math.Sqrt(board.Length);
    }

    public BingoPosition[] Board { get; init; }

    public IEnumerable<BingoPosition[]> Rows()
    {
        for (int boardIndex = 0; boardIndex < Board.Length; boardIndex += BoardSize)
        {
            yield return Board[boardIndex..(boardIndex + BoardSize)];
        }
    }

    public IEnumerable<BingoPosition[]> Columns()
    {
        for (int boardColumn = 0; boardColumn < Board.Length / BoardSize; boardColumn++)
        {
            var boardSize = BoardSize;
            var currentBoard = Board;
            yield return Enumerable
                .Range(0, BoardSize)
                .Select(boardIndex => boardColumn + boardIndex * boardSize)
                .Select(index => currentBoard[index])
                .ToArray()
            ;

            /*yield return new BingoPosition[] {
                Board[boardColumn + 0 * BoardSize],
                Board[boardColumn + 1 * BoardSize],
                Board[boardColumn + 2 * BoardSize],
                Board[boardColumn + 3 * BoardSize],
                Board[boardColumn + 4 * BoardSize],
            };*/
        }
    }

    public bool CheckWinCondition()
    {
        var winConditions = Rows().Concat(Columns());
        return winConditions.Any(condition => condition.All(pos => pos.IsDrawn));
    }

    public void MarkPosition(int number)
    {
        for (var position = 0; position < Board.Length; position++)
        {
            if (Board[position].Value == number)
            {
                Board[position] = new BingoPosition(number, true);
                return;
            }
        }
    }

    public int CalculateScore(int number)
    {
        return Board
            .Where(x => !x.IsDrawn)
            .Sum(x => x.Value) * number;
    }
}

public readonly struct BingoPosition
{
    public BingoPosition(int value, bool isDrawn)
    {
        Value = value;
        IsDrawn = isDrawn;
    }

    public int Value { get; init; }
    public bool IsDrawn { get; init; }
}