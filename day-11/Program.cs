/*
 * Advent of Code 2024, Day 11
 */

var input = File.ReadAllText("input.txt").Split(' ').Select(long.Parse).ToArray();

var part1Solution = input.AsParallel().Sum(x => Blink(x, 25));
Console.WriteLine($"Part 1: {part1Solution}");

var part2Solution = input.AsParallel().Sum(x => Blink(x, 75));
Console.WriteLine($"Part 2: {part2Solution}");

return;

long Blink(long stone, int blinkCount, int blinks = 0)
{
    while (blinks < blinkCount)
    {
        if (stone == 0)
        {
            stone = 1;
            blinks += 1;
            continue;
        }

        var digits = stone.ToString();
        if (digits.Length % 2 == 0)
            return Blink(long.Parse(digits[..(digits.Length / 2)]), blinkCount, blinks + 1) +
                   Blink(long.Parse(digits[(digits.Length / 2)..]), blinkCount, blinks + 1);

        stone *= 2024;
        blinks += 1;
    }

    return 1;
}