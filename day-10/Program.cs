/*
 * Advent of Code 2024, Day 10
 */

var input = File.ReadAllLines("input.txt");

// Cut the newlines from the end and convert the digits to numbers -- '0' is ASCII 48
var map = new byte[input.Length][];
var trailheads = new List<(int Row, int Col)>();
foreach (var (index, line) in input.Index())
{
    map[index] = line.TrimEnd().Select(c => (byte)(c - 48)).ToArray();
    // Add all the zeroes to a list of the trailheads
    trailheads.AddRange(map[index].Index().Where(x => x.Item == 0).Select(x => (index, x.Index)));
}

Console.WriteLine($"Found {trailheads.Count} trailheads");

var part1Answer = 0;
var part2Answer = 0;

foreach (var trailhead in trailheads)
{
    var peaks = SeekPeaks(trailhead.Row, trailhead.Col).ToArray();
    part1Answer += peaks.Distinct().Count();
    part2Answer += peaks.Length;
}

Console.WriteLine($"Part 1: {part1Answer}");
Console.WriteLine($"Part 2: {part2Answer}");

return;

IEnumerable<(int Row, int Col)> SeekPeaks(int row, int col, int height = 0)
{
    // If we are at height 9, we have found a path to a peak!
    if (height == 9) return [(row, col)];

    // For one step in every direction
    return new[] {(x:0, y:-1), (x:1,y:0), (x:0, y:1), (x:-1, y:0)}
        // If moving that way is inside the map bounds
        .Where(move => row + move.y >= 0 &&
                       row + move.y < map.Length &&
                       col + move.x >= 0 &&
                       col + move.x < map[row + move.y].Length &&
                       // And the height at that location is one step up
                       map[row + move.y][col + move.x] == height + 1)
        // Recursively seek the next step on the trail
        .SelectMany(move => SeekPeaks(row + move.y, col + move.x, height + 1));
}