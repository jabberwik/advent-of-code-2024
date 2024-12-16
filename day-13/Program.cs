/*
 * Advent of Code 2024, Day 13
 */

using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var buttonPattern = new Regex(@"Button [AB]: X\+(\d+), Y\+(\d+)");
var prizePattern = new Regex(@"Prize: X=(\d+), Y=(\d+)");

var totalCost = 0L;

for (var line = 0; line < input.Length; line += 4)
{
  var lineA = buttonPattern.Match(input[line]);
  var lineB = buttonPattern.Match(input[line + 1]);
  var prize = prizePattern.Match(input[line + 2]);
  
  var m = new
  {
      XA = long.Parse(lineA.Groups[1].Value),
      YA = long.Parse(lineA.Groups[2].Value),
      XB = long.Parse(lineB.Groups[1].Value),
      YB = long.Parse(lineB.Groups[2].Value),
      TX = long.Parse(prize.Groups[1].Value) + 10000000000000, // Added for Part 2
      TY = long.Parse(prize.Groups[2].Value) + 10000000000000, // Added for Part 2
  };
  
  // Use Cramer's Rule to solve the system
  
  var determinant = m.XA * m.YB - m.XB * m.YA;
  if (determinant == 0)
  {
      Console.WriteLine($"Machine {line}: No solution");
      continue;
  }

  // This is all integer division, so use DivRem to make sure we're actually getting whole results
  var (aPresses, aRemainder) = Math.DivRem(m.TX * m.YB - m.XB * m.TY, determinant);
  var (bPresses, bRemainder) = Math.DivRem(m.XA * m.TY - m.TX * m.YA, determinant);

  if (aRemainder != 0 || bRemainder != 0)
  {
      Console.WriteLine($"Machine {line}: No whole solution");
      continue;
  }

  var cost = 3 * aPresses + bPresses;
  
  Console.WriteLine($"Machine {line}: A = {aPresses}, B = {bPresses}, Cost = {cost}");
  
  totalCost += cost;
}

Console.WriteLine(totalCost);