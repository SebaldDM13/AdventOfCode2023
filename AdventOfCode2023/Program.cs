using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public static partial class Program
{
    [GeneratedRegex(@".*?(\d|zero|one|two|three|four|five|six|seven|eight|nine)")]
    private static partial Regex FirstDigit();

    [GeneratedRegex(@".*(\d|zero|one|two|three|four|five|six|seven|eight|nine)")]
    private static partial Regex LastDigit();

    static void Main()
    {
        Console.WriteLine("Advent of Code 2023");
        Console.WriteLine();

        Console.WriteLine("Day 01: Part 1");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day01.txt"));
        Console.WriteLine(lines.Select(s => (s.First(c => Char.IsDigit(c)) - '0') * 10 + s.Last(c => Char.IsDigit(c)) - '0').Sum());
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

        int sum = 0;
        foreach (string line in lines)
        {
            sum += DigitToValue(FirstDigit().Match(line).Groups[1].Value) * 10;
            sum += DigitToValue(LastDigit().Match(line).Groups[1].Value);
        }
        Console.WriteLine(sum);
        Console.WriteLine();

        Console.WriteLine("Day 02: Part 1");
        lines = File.ReadAllLines(Path.Combine("Input", "Day02.txt"));
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
}
