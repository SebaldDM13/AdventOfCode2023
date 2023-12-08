using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public static partial class Program
{
    public static void Main()
    {
        Console.WriteLine("Advent of Code 2023");
        Console.WriteLine();
        foreach (MethodInfo methodInfo in typeof(Program).GetMethods().Where(m => m.Name.StartsWith("Day")).OrderBy(m => m.Name))
        {
            methodInfo.Invoke(null, null);
        }
    }

    [GeneratedRegex(@".*?(\d|zero|one|two|three|four|five|six|seven|eight|nine)")]
    private static partial Regex FirstDigit();

    [GeneratedRegex(@"(\d|zero|one|two|three|four|five|six|seven|eight|nine).*?", RegexOptions.RightToLeft)]
    private static partial Regex LastDigit();

    public static void Day01()
    {
        Console.Write("Day 01: Part 1: ");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day01.txt"));
        Console.WriteLine(lines.Sum(s => (s.First(Char.IsDigit) - '0') * 10 + s.Last(Char.IsDigit) - '0'));

        Console.Write("Day 01: Part 2: ");
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

    public static void Day02()
    {
        Console.Write("Day 02: Part 1: ");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day02.txt"));
        int gameNumberSum = 0;
        int setPowerSum = 0;
        Dictionary<string, int> cubes = [];
        foreach (string line in lines)
        {
            (cubes["red"], cubes["green"], cubes["blue"]) = (0, 0, 0);
            int space = line.IndexOf(' ');
            int colon = line.IndexOf(':', space);
            int gameNumber = int.Parse(line[(space + 1)..colon]);
            foreach (string round in line[(colon + 1)..].Split(';', StringSplitOptions.TrimEntries))
            {
                foreach (string reveal in round.Split(',', StringSplitOptions.TrimEntries))
                {
                    space = reveal.IndexOf(' ');
                    int count = int.Parse(reveal[..space]);
                    string color = reveal[(space + 1)..];
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

        Console.Write("Day 02: Part 2: ");
        Console.WriteLine(setPowerSum);
        Console.WriteLine();
    }

    public static void Day03()
    {
        Console.Write("Day 03: Part 1: Method 1: ");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day03.txt"));
        int sumOfPartNumbers = 0;

        char At(int row, int col)
        {
            if (row < 0 || row >= lines.Length || col < 0 || col >= lines[0].Length)
                return '.';

            return lines[row][col];
        }

        static bool IsSymbol(char c) => !(c == '.' || char.IsDigit(c));

        for (int row = 0; row < lines.Length; row++)
        {
            bool isAdjacentToSymbol = false;
            for (int col = 0; col < lines[row].Length; col++)
            {
                switch (lines[row][col])
                {
                    case >= '0' and <= '9':
                        int end = col;
                        do
                        {
                            isAdjacentToSymbol |= IsSymbol(At(row - 1, end)) || IsSymbol(At(row + 1, end));
                            end++;
                        } while (end < lines[row].Length && lines[row][end] >= '0' && lines[row][end] <= '9');

                        isAdjacentToSymbol |= IsSymbol(At(row - 1, end)) || IsSymbol(At(row, end)) || IsSymbol(At(row + 1, end));
                        if (isAdjacentToSymbol)
                        {
                            sumOfPartNumbers += int.Parse(lines[row][col..end]);
                        }

                        col = end - 1;
                        break;
                    default:
                        isAdjacentToSymbol = IsSymbol(At(row - 1, col)) || IsSymbol(At(row, col)) || IsSymbol(At(row + 1, col));
                        break;
                }
            }
        }

        Console.WriteLine(sumOfPartNumbers);

        Console.Write("Day 03: Part 1: Method 2: ");
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

        Console.Write("Day 03: Part 2: ");
        Console.WriteLine(symbolMap.Where(s => s.symbol == '*')
                                   .Select(s => numberMap.Where(n => AreAdjacent(n, s)))
                                   .Where(nGroup => nGroup.Count() == 2)
                                   .Sum(nGroup => nGroup.Aggregate(1, (acc, n) => acc * int.Parse(lines[n.row][n.range]))));
        Console.WriteLine();
    }

    public static void Day04()
    {
        Console.Write("Day 04: Part 1: ");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day04.txt"));
        int[] score = new int[lines.Length];
        int[] copyCount = new int[lines.Length];
        for (int i = lines.Length - 1; i >= 0; i--)
        {
            int offset = 0;
            copyCount[i] = 1;
            string[] parts = lines[i].Split(':', '|');            
            foreach (string s in parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Intersect(parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)))
            {                
                score[i] = (score[i] == 0) ? 1 : 2 * score[i];
                offset++;
                copyCount[i] += copyCount[i + offset];
            }
        }

        Console.WriteLine(score.Sum());

        Console.Write("Day 04: Part 2: ");
        Console.WriteLine(copyCount.Sum());
        Console.WriteLine();
    }

    public static void Day05()
    {
        Console.Write("Day 05: Part 1: ");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day05.txt"));
        long[] number = lines[0].Split(' ').Skip(1).Select(long.Parse).ToArray();
        Dictionary<string, List<(long destination, long source, long length)>> conversionMap = [];

        string currentMap = string.Empty;
        foreach (string line in lines[1..])
        {
            if (line.Contains(':'))
            {
                currentMap = line[0..line.IndexOf(' ')];
                conversionMap[currentMap] = [];
            }
            else if (line.Length > 0)
            {
                string[] part = line.Split(' ');
                conversionMap[currentMap].Add((long.Parse(part[0]), long.Parse(part[1]), long.Parse(part[2])));
            }
        }

        string sourceType = "seed";
        while (sourceType != "location")
        {
            string conversionKey = conversionMap.Single(m => m.Key.StartsWith(sourceType)).Key;
            string destinationType = conversionKey[(conversionKey.LastIndexOf('-') + 1)..];

            for (int i = 0; i < number.Length; i++)
            {
                (long destination, long source, _) = conversionMap[conversionKey].Find(m => m.source <= number[i] && number[i] < m.source + m.length);
                number[i] += destination - source;
            }

            sourceType = destinationType;
        }

        Console.WriteLine(number.Min());

        Console.Write("Day 05: Part 2: ");
        sourceType = "seed";
        Dictionary<string, List<(long start, long length)>> numberMap = new() { { sourceType, [] } };
        number = lines[0].Split(' ').Skip(1).Select(long.Parse).ToArray();
        for (int i = 0; i < number.Length; i += 2)
        {
            numberMap[sourceType].Add((number[i], number[i + 1]));
        }

        while (sourceType != "location")
        {
            string conversionKey = conversionMap.Single(m => m.Key.StartsWith(sourceType)).Key;
            string destinationType = conversionKey[(conversionKey.LastIndexOf('-') + 1)..];
            numberMap[destinationType] = [];
            HashSet<long> edgeSet = [
                .. numberMap[sourceType].Select(m => m.start),
                .. numberMap[sourceType].Select(m => m.start + m.length),
                .. conversionMap[conversionKey].Select(m => m.source),
                .. conversionMap[conversionKey].Select(m => m.source + m.length)];

            long[] edges = [.. edgeSet.Order()];
            for (int i = 0; i < edges.Length - 1; i++)
            {
                if (numberMap[sourceType].Any(m => m.start <= edges[i] && edges[i] < m.start + m.length))
                {
                    (long destination, long source, _) = conversionMap[conversionKey].Find(m => m.source <= edges[i] && edges[i] < m.source + m.length);
                    numberMap[destinationType].Add((edges[i] + destination - source, edges[i + 1] - edges[i]));
                }
            }

            sourceType = destinationType;
        }

        Console.WriteLine(numberMap["location"].Min(n => n.start));
        Console.WriteLine();
    }

    public static void Day06()
    {
        Console.Write("Day 06: Part 1: ");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day06.txt"));
        IEnumerable<int> times = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        IEnumerable<int> distances = lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        int product = 1;
        foreach ((int t, int d) in times.Zip(distances))
        {
            product *= Enumerable.Range(0, t + 1).Count(n => n * (t - n) > d);
        }

        Console.WriteLine(product);

        Console.Write("Day 06: Part 2: ");
        long time = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
        long distance = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));
        long count = 0;
        for (long n = 0; n <= time; n++)
        {
            if (n * (time - n) > distance)
            {
                count++;
            }
        }

        Console.WriteLine(count);
        Console.WriteLine();
    }

    public static void Day07()
    {
        Console.Write("Day 07: Part 1: ");

        static int Part1CardValue(char card) => card switch
        {
            >= '2' and <= '9' => card - '0',
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => 0
        };

        static int Part1HandStrength(string hand)
        {
            int[] cardCounts = new int[13];
            foreach (char c in hand)
            {
                cardCounts[Part1CardValue(c) - 2]++;
            }

            if (cardCounts.Contains(5))
                return 6;

            if (cardCounts.Contains(4))
                return 5;

            if (cardCounts.Contains(3) && cardCounts.Contains(2))
                return 4;

            if (cardCounts.Contains(3))
                return 3;

            if (cardCounts.Count(c => c == 2) == 2)
                return 2;

            if (cardCounts.Contains(2))
                return 1;

            return 0;
        }

        static int Part1HandComparison(string a, string b)
        {
            int relativeValue = Part1HandStrength(a) - Part1HandStrength(b);
            if (relativeValue != 0)
                return relativeValue;

            for (int i = 0; i < 5; i++)
            {
                if (a[i] != b[i])
                    return Part1CardValue(a[i]) - Part1CardValue(b[i]);
            }

            return 0;
        }
                
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day07.txt"));
        List<(string hand, int bid)> plays = [.. lines.Select(s => s.Split(' ')).Select(t => (t[0], int.Parse(t[1])))];
        plays.Sort((a, b) => Part1HandComparison(a.hand, b.hand));

        int winnings = 0;
        for (int i = 0; i < plays.Count; i++)
        {
            winnings += (i + 1) * plays[i].bid;
        }

        Console.WriteLine(winnings);

        Console.Write("Day 07: Part 2: ");

        static int Part2CardValue(char card) => card switch
        {
            'J' => 1,
            >= '2' and <= '9' => card - '0',
            'T' => 10,
            'Q' => 11,
            'K' => 12,
            'A' => 13,
            _ => 0
        };

        static int Part2HandStrength(string hand)
        {
            int[] cardCounts = new int[13];
            foreach (char c in hand)
            {
                cardCounts[Part2CardValue(c) - 1]++;
            }

            if (cardCounts[1..].Max() + cardCounts[0] >= 5) 
                return 6;

            if (cardCounts[1..].Max() + cardCounts[0] == 4)
                return 5;

            if ((cardCounts.Contains(3) && cardCounts.Contains(2)) ||
                (cardCounts[1..].Count(c => c == 2) == 2 && cardCounts[0] == 1))
                return 4;

            if (cardCounts[1..].Max() + cardCounts[0] == 3)
                return 3;

            if (cardCounts.Count(c => c == 2) == 2)
                return 2;

            if (cardCounts[1..].Max() + cardCounts[0] == 2)
                return 1;

            return 0;
        }

        static int Part2HandComparison(string a, string b)
        {
            int relativeValue = Part2HandStrength(a) - Part2HandStrength(b);
            if (relativeValue != 0)
                return relativeValue;

            for (int i = 0; i < 5; i++)
            {
                if (a[i] != b[i])
                    return Part2CardValue(a[i]) - Part2CardValue(b[i]);
            }

            return 0;
        }

        plays.Sort((a, b) => Part2HandComparison(a.hand, b.hand));

        winnings = 0;
        for (int i = 0; i < plays.Count; i++)
        {
            winnings += (i + 1) * plays[i].bid;
        }

        Console.WriteLine(winnings);
        Console.WriteLine();
    }

    public static void Day08()
    {
        Console.Write("Day 08: Part 1: ");
        string[] lines = File.ReadAllLines(Path.Combine("Input", "Day08.txt"));
        string directions = lines[0];
        Dictionary<string, (string left, string right)> map = lines[2..].ToDictionary(s => s[0..3], s => (s[7..10], s[12..15]));

        (int moves, string destination) MoveToZLocation(string start)
        {
            int moves = 0;
            string destination = start;
            do
            {
                switch (directions[moves % directions.Length])
                {
                    case 'L':
                        destination = map[destination].left;
                        break;
                    case 'R':
                        destination = map[destination].right;
                        break;
                }

                moves++;

            } while (!destination.EndsWith('Z'));

            return (moves, destination);
        }

        (int moves, string destination) = MoveToZLocation("AAA");
        Debug.Assert(destination == "ZZZ");
        Console.WriteLine(moves);

        Console.Write("Day 08: Part 2: ");        
        IEnumerable<string> aLocations = map.Keys.Where(s => s.EndsWith('A'));
        IEnumerable<(int moves, string destination)> zRoute1 = aLocations.Select(MoveToZLocation);
        IEnumerable<(int moves, string destination)> zRoute2 = zRoute1.Select(m => MoveToZLocation(m.destination));

        Debug.Assert(zRoute1.All(m => m.moves % directions.Length == 0));
        Debug.Assert(Enumerable.SequenceEqual(zRoute1, zRoute2));

        long LeastCommonMultiple(long a, long b)
        {
            (long gcd, long tmp) = (a, b);
            while (tmp != 0)
            {
                (gcd, tmp) = (tmp, gcd % tmp);
            }
            return a * b / gcd;
        }

        int[] cycleLengths = [.. zRoute1.Select(m => m.moves)];
        long lcm = cycleLengths[0];
        for (int i = 1; i < cycleLengths.Length; i++)
        {
            lcm = LeastCommonMultiple(lcm, cycleLengths[i]);
        }

        Console.WriteLine(lcm);
    }
}
