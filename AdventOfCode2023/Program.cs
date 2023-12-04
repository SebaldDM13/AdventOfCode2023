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
    }

    [GeneratedRegex(@".*?(\d|zero|one|two|three|four|five|six|seven|eight|nine)")]
    private static partial Regex FirstDigit();

    [GeneratedRegex(@"(\d|zero|one|two|three|four|five|six|seven|eight|nine).*?", RegexOptions.RightToLeft)]
    private static partial Regex LastDigit();

    static void Day01()
    {
        Console.WriteLine("Day 01: Part 1");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day01.txt"));
        Console.WriteLine(lines.Select(s => (s.First(Char.IsDigit) - '0') * 10 + s.Last(Char.IsDigit) - '0').Sum());
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

        int sumOfCalibrationValues = 0;
        foreach (string line in lines)
        {
            sumOfCalibrationValues += DigitToValue(FirstDigit().Match(line).Groups[1].Value) * 10;
            sumOfCalibrationValues += DigitToValue(LastDigit().Match(line).Groups[1].Value);
        }
        Console.WriteLine(sumOfCalibrationValues);
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

    static void Day03()
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
}
