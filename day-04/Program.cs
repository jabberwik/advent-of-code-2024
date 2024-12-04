/*
 * Advent of Code 2024, Day 4
 */

// Read the input file into a two-dimensional array of chars
// (Yes it's technically a jagged array but it's easier to fill this way)
var input = File.ReadAllLines("input.txt");
char[][] source = input.Select(row => row.ToCharArray()).ToArray();

// Part 1: Given the position of an 'X' and the lengths of the X and Y arrays,
//         return sets of search coordinates for the remaining characters in
//         all possible directions
IEnumerable<(char Char, int X, int Y)[]> GetXmasSearches(int x, int y, int xlen, int ylen)
{
  // N
  if (y >= 3)
  {
    yield return [
      ('M', x, y - 1),
      ('A', x, y - 2),
      ('S', x, y - 3)
    ];
  }
  
  // NE
  if (x < xlen - 3 && y >= 3)
  {
    yield return [
      ('M', x + 1, y - 1),
      ('A', x + 2, y - 2),
      ('S', x + 3, y - 3)
    ];
  }
  
  // E
  if (x < xlen - 3)
  {
    yield return [
      ('M', x + 1, y),
      ('A', x + 2, y),
      ('S', x + 3, y)
    ];
  }
  
  // SE
  if (x < xlen - 3 && y < ylen - 3)
  {
    yield return [
      ('M', x + 1, y + 1),
      ('A', x + 2, y + 2),
      ('S', x + 3, y + 3)
    ];
  }
  
  // S
  if (y < ylen - 3)
  {
    yield return [
      ('M', x, y + 1),
      ('A', x, y + 2),
      ('S', x, y + 3)
    ];
  }
  
  // SW
  if (x >= 3 && y < ylen - 3)
  {
    yield return [
      ('M', x - 1, y + 1),
      ('A', x - 2, y + 2),
      ('S', x - 3, y + 3)
    ];
  }
  
  // W
  if (x >= 3)
  {
    yield return [
      ('M', x - 1, y),
      ('A', x - 2, y),
      ('S', x - 3, y)
    ];
  }
  
  // NW
  if (x >= 3 && y >= 3)
  {
    yield return [
      ('M', x - 1, y - 1),
      ('A', x - 2, y - 2),
      ('S', x - 3, y - 3)
    ];
  }
}

var xmasCount = 0L;
for (var x = 0; x < source.Length; x++)
for (var y = 0; y < source[x].Length; y++)
{
  if (source[x][y] != 'X') continue;

  foreach (var search in GetXmasSearches(x, y, source.Length, source[x].Length))
  {
    if (search.All(target => source[target.X][target.Y] == target.Char)) xmasCount++;
  }
}
Console.WriteLine(xmasCount);


// Part 2: Given the position of an 'A', return the sets of coordinates to
//         seach for the remaining characters in all directions.
//         We are not bounds-checking here!! Assuming the input coordinates
//         keep the necessary one-character pad.
IEnumerable<(char Char, int X, int Y)[]> GetXShapedMasSearches(int x, int y)
{
  // ↘️↙
  yield return [
    ('M', x - 1, y - 1),
    ('S', x + 1, y + 1),
    ('M', x + 1, y - 1),
    ('S', x - 1, y + 1),
  ];
  // ↗️↖️
  yield return [
    ('M', x - 1, y + 1),
    ('S', x + 1, y - 1),
    ('M', x + 1, y + 1),
    ('S', x - 1, y - 1),
  ];
  // ↘️↗️
  yield return [
    ('M', x - 1, y + 1),
    ('S', x + 1, y - 1),
    ('M', x - 1, y - 1),
    ('S', x + 1, y + 1),
  ];
  // ↙↖️
  yield return [
    ('M', x + 1, y - 1),
    ('S', x - 1, y + 1),
    ('M', x + 1, y + 1),
    ('S', x - 1, y - 1),
  ];
}

var xShapedMasCount = 0L;
// Loop from 1 to Length - 1 because we always need a pad of 1 char around the 'A'
for (var x = 1; x < source.Length - 1; x++)
for (var y = 1; y < source[x].Length - 1; y++)
{
  if (source[x][y] != 'A') continue;
    
  foreach (var search in GetXShapedMasSearches(x, y))
  {
    if (search.All(target => source[target.X][target.Y] == target.Char)) xShapedMasCount++;
  }
}
Console.WriteLine(xShapedMasCount);
