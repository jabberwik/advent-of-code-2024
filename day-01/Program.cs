/*
 * Advent of Code 2024, Day 1
 */

// Read the input file into two separate arrays of int64s, sorted ascending
var input = File.ReadAllLines("input.txt");
var rows = input.Select(row => row.Split(' ', StringSplitOptions.RemoveEmptyEntries));
var leftList = rows.Select(row => long.Parse(row[0])).OrderBy(x => x).ToArray();
var rightList = rows.Select(row => long.Parse(row[1])).OrderBy(x => x).ToArray();

// For part 1, sum the differences between each list element.
var joinedData = leftList.Zip(rightList);
var distances = joinedData.Select(row => Math.Abs(row.First - row.Second));
var part1Answer = distances.Sum();
Console.WriteLine(part1Answer);

// For part 2, sum each left entry multiplied by the number of times it appears in the right list.
var part2Answer = 0L;

// Since the right list is sorted, we only need to iterate through it once, but separately from the left list
var rightIndex = 0;
foreach(var left in leftList)
{
  var occurrences = 0;
  // Advance the right list until we're at the left value (or past it already)
  while(rightIndex < rightList.Length && rightList[rightIndex] < left) rightIndex++;
  // Start counting the number of matches
  while(rightIndex < rightList.Length && rightList[rightIndex] == left)
  {
    occurrences++;
    rightIndex++;
  }
  part2Answer += left * occurrences;
}
Console.WriteLine(part2Answer);
