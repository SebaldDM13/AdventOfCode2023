using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public static partial class Program
{
    [GeneratedRegex(@".*?(1|2|3|4|5|6|7|8|9|one|two|three|four|five|six|seven|eight|nine)")]
    private static partial Regex FirstDigit();

    [GeneratedRegex(@".*(1|2|3|4|5|6|7|8|9|one|two|three|four|five|six|seven|eight|nine)")]
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
            _ => 0
        };

        int sum = 0;
        foreach (string line in lines)
        {
            sum += 10 * DigitToValue(FirstDigit().Match(line).Groups[1].Value);
            sum += DigitToValue(LastDigit().Match(line).Groups[1].Value);
        }
        Console.WriteLine(sum);
    }
}
