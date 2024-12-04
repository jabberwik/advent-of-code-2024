/*
 * Advent of Code 2024, Day 3
 */

using System.Text.RegularExpressions;

// Read the file as one big input string
var input = File.ReadAllText("input.txt");

// Part 1: "Just" a regex to find all the mul(x,y) tokens
var mulPattern = new Regex(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled);

var mulMatches = mulPattern.Matches(input);
var part1Answer = 0L;
foreach(Match match in mulMatches)
{
  var arg1 = int.Parse(match.Groups[1].Value);
  var arg2 = int.Parse(match.Groups[2].Value);
  part1Answer += arg1 * arg2;
}
Console.WriteLine(part1Answer);

// Part 2: Additional tokens to find

// Tweaking the regex a bit; we want to strictly match at the given start point
var mul2pattern = new Regex(@"\Gmul\((\d+),(\d+)\)", RegexOptions.Compiled);

var cursor = 0;
var mulEnabled = true;
var part2Answer = 0L;

// Process through the input string. Stop before the end so that we don't try
// to read substrings that extend past the end of the input. 7 chars is not
// enough to fit a mul(), which is the only token that would matter.
while (cursor < input.Length - 7)
{
  if (input.Substring(cursor, 4) == "do()")
  {
    mulEnabled = true;
    cursor += 4;
    continue;
  }

  if (input.Substring(cursor, 7) == "don't()")
  {
    mulEnabled = false;
    cursor += 7;
    continue;
  }

  var match = mul2pattern.Match(input, cursor);

  if (match.Success)
  {
    if (mulEnabled)
    {
      var arg1 = int.Parse(match.Groups[1].Value);
      var arg2 = int.Parse(match.Groups[2].Value);
      part2Answer += arg1 * arg2;
    }

    cursor += match.Length;
    continue;
  }

  cursor += 1;
}

Console.WriteLine(part2Answer);