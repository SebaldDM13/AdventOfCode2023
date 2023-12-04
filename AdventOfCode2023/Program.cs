using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public static partial class Program
{
    static void Main()
    {
        Console.WriteLine("Advent of Code 2023");
        Console.WriteLine();
        Day01();
        Day02();
        Day03();
        Day04();
    }

    [GeneratedRegex(@".*?(\d|zero|one|two|three|four|five|six|seven|eight|nine)")]
    private static partial Regex FirstDigit();

    [GeneratedRegex(@"(\d|zero|one|two|three|four|five|six|seven|eight|nine).*?", RegexOptions.RightToLeft)]
    private static partial Regex LastDigit();

    static void Day01()
    {
        Console.WriteLine("Day 01: Part 1");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day01.txt"));
        Console.WriteLine(lines.Sum(s => (s.First(Char.IsDigit) - '0') * 10 + s.Last(Char.IsDigit) - '0'));
        Console.WriteLine();

        Console.WriteLine("Day 01: Part 2");
        static int DigitToValue(string input) => input switch
        {
            "1" or "one" => 1,
            "2" or "two" => 2,
            "3" or "three" => 3,
            "4" or "four" => 4,
            "5" or "five" => 5,
            "6" or "six" => 6,
            "7" or "seven" => 7,
            "8" or "eight" => 8,
            "9" or "nine" => 9,
            "0" or "zero" or _ => 0
        };

        Console.WriteLine(lines.Sum(s => DigitToValue(FirstDigit().Match(s).Groups[1].Value) * 10 + DigitToValue(LastDigit().Match(s).Groups[1].Value)));
        Console.WriteLine();
    }

    static void Day02()
    {
        Console.WriteLine("Day 02: Part 1");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day02.txt"));
        int gameNumberSum = 0;
        int setPowerSum = 0;
        Dictionary<string, int> cubes = [];
        foreach (string line in lines)
        {
            (cubes["red"], cubes["green"], cubes["blue"]) = (0, 0, 0);
            string[] gameSplit = line.Split(": ");
            int gameNumber = int.Parse(gameSplit[0].Split(' ')[1]);
            foreach (string round in gameSplit[1].Split("; "))
            {
                foreach (string reveal in round.Split(", "))
                {
                    string[] countAndColor = reveal.Split(' ');
                    int count = int.Parse(countAndColor[0]);
                    string color = countAndColor[1];
                    cubes[color] = Math.Max(cubes[color], count);
                }
            }

            if (cubes["red"] <= 12 && cubes["green"] <= 13 && cubes["blue"] <= 14)
            {
                gameNumberSum += gameNumber;
            }

            setPowerSum += cubes["red"] * cubes["green"] * cubes["blue"];
        }

        Console.WriteLine(gameNumberSum);
        Console.WriteLine();

        Console.WriteLine("Day 02: Part 2");
        Console.WriteLine(setPowerSum);
        Console.WriteLine();
    }

    static void Day03Part1VersionA()
    {
        Console.WriteLine("Day 03: Part 1");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day03.txt"));
        StringBuilder numberText = new();
        int sumOfPartNumbers = 0;
        bool isAdjacentToSymbol = false;

        char At(int row, int col)
        {
            if (row < 0 || row >= lines.Length || col < 0 || col >= lines[0].Length)
                return '.';

            return lines[row][col];
        }

        static bool IsSymbol(char c) => !(c == '.' || char.IsDigit(c));
        void EndOfNumber()
        {
            if (numberText.Length > 0 && isAdjacentToSymbol)
            {
                sumOfPartNumbers += int.Parse(numberText.ToString());
            }

            numberText.Clear();
        }

        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                char top = At(row - 1, col);
                char middle = At(row, col);
                char bottom = At(row + 1, col);

                if (char.IsDigit(middle))
                {
                    isAdjacentToSymbol = isAdjacentToSymbol || IsSymbol(top) || IsSymbol(bottom);
                    numberText.Append(middle);
                }
                else if (middle == '.')
                {
                    isAdjacentToSymbol = isAdjacentToSymbol || IsSymbol(top) || IsSymbol(bottom);
                    EndOfNumber();
                    isAdjacentToSymbol = IsSymbol(top) || IsSymbol(bottom);
                }
                else
                {
                    isAdjacentToSymbol = true;
                    EndOfNumber();
                }
            }

            EndOfNumber();
            isAdjacentToSymbol = false;
        }

        Console.WriteLine(sumOfPartNumbers);
        Console.WriteLine();
    }

    static void Day03()
    {
        Console.WriteLine("Day 03: Part 1");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day03.txt"));
        List<(int row, Range range)> numberMap = [];
        List<(int row, int col, char symbol)> symbolMap = [];

        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                switch (lines[row][col])
                {
                    case >= '0' and <= '9':
                        int end = col + 1;
                        while (end < lines[row].Length && lines[row][end] >= '0' && lines[row][end] <= '9')
                        {
                            end++;
                        }

                        numberMap.Add((row, col..end));
                        col = end - 1;
                        break;
                    case '.':
                        break;
                    default:
                        symbolMap.Add((row, col, lines[row][col]));
                        break;
                }
            }
        }

        static bool AreAdjacent((int row, Range range) n, (int row, int col, char symbol) s) => Math.Abs(n.row - s.row) <= 1 && n.range.Start.Value - 1 <= s.col && s.col <= n.range.End.Value;
        Console.WriteLine(numberMap.Where(n => symbolMap.Any(s => AreAdjacent(n, s))).Sum(n => int.Parse(lines[n.row][n.range])));
        Console.WriteLine();

        Console.WriteLine("Day 03: Part 2");
        Console.WriteLine(symbolMap.Where(s => s.symbol == '*')
                                   .Select(s => numberMap.Where(n => AreAdjacent(n, s)))
                                   .Where(nGroup => nGroup.Count() == 2)
                                   .Sum(nGroup => nGroup.Aggregate(1, (acc, n) => acc * int.Parse(lines[n.row][n.range]))));
        Console.WriteLine();
    }

    static void Day04()
    {
        Console.WriteLine("Day 04: Part 1");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day04.txt"));
        int[] matchCount = new int[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(':', '|');
            matchCount[i] = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                                    .Intersect(parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
                                    .Count();
        }

        Console.WriteLine(matchCount.Where(m => m > 0).Sum(m => 1 << (m - 1)));
        Console.WriteLine();

        Console.WriteLine("Day 04: Part 2");
        int[] copyCount = new int[lines.Length];
        for (int i = copyCount.Length - 1; i >= 0; i--)
        {
            copyCount[i] = 1 + copyCount[(i + 1)..(i + 1 + matchCount[i])].Sum();
        }

        Console.WriteLine(copyCount.Sum());
        Console.WriteLine();
    }
}
