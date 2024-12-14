/*
 * Advent of Code 2024, Day 11
 */

using System.Collections.Concurrent;

var input = File.ReadAllText("input.txt").Split(' ').Select(long.Parse).ToArray();

// We see lots of repeat evaluations so cache the results
ConcurrentDictionary<(long Stone, int RemainingBlinks), long> cache = new();

var part1Solution = input.AsParallel().Sum(x => Blink(x, 25));
Console.WriteLine($"Part 1: {part1Solution}");

var part2Solution = input.AsParallel().Sum(x => Blink(x, 75));
Console.WriteLine($"Part 2: {part2Solution}");

return;

long Blink(long stone, int blinkCount, int blinks = 0)
{
    // Check if we have done this before
    var cacheKey = (stone, blinkCount - blinks);
    if (cache.TryGetValue(cacheKey, out var cachedResult)) return cachedResult;

    // If we're at the target blink depth, we're done as one stone
    if (blinks >= blinkCount) return 1;

    long result;
    if (stone == 0)
    {
        result = Blink(1, blinkCount, blinks + 1);
    }
    else
    {
        var digits = CountDigits(stone);
        if (digits % 2 == 0)
        {
            // Split number mathematically
            var leftHalf = stone / (long)Math.Pow(10, digits / 2);
            var rightHalf = stone % (long)Math.Pow(10, digits / 2);

            result = Blink(leftHalf, blinkCount, blinks + 1) +
                     Blink(rightHalf, blinkCount, blinks + 1);
        }
        else
        {
            result = Blink(stone * 2024, blinkCount, blinks + 1);
        }
    }

    // Cache the result before returning
    cache[cacheKey] = result;
    return result;
}

int CountDigits(long number)
{
    if (number == 0) return 1;
    return (int)Math.Floor(Math.Log10(number)) + 1;
}