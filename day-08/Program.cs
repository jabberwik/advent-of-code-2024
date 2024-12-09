/*
 * Advent of Code 2024, Day 8
 */

var input = File.ReadAllLines("input.txt");

var mapHeight = input.Length;
var mapWidth = input[0].Length;

var antennas = new List<(int X, int Y, char Freq)>();

for (var line = 0; line < input.Length; line++)
for (var col = 0; col < input[line].Length; col++)
{
  if (input[line][col] == '.') continue;
  
  antennas.Add((col, line, input[line][col]));
}

var antinodes = new HashSet<(int X, int Y)>();
var resonantNodes = new HashSet<(int X, int Y)>();

foreach (var antennaGroup in antennas.GroupBy(a => a.Freq).Select(g => (Freq: g.Key, Antennas: g.ToArray())))
{
  Console.WriteLine($"Frequency {antennaGroup.Freq} has {antennaGroup.Antennas.Length} antennas");

  // For every combination of 2 antennas
  var combinations = Enumerable.Range(0, antennaGroup.Antennas.Length)
    .SelectMany(i => Enumerable.Range(i + 1, antennaGroup.Antennas.Length - i - 1)
        .Select(j => (antennaGroup.Antennas[i], antennaGroup.Antennas[j]))
    )
    .ToList();

  // Part 1: Just the two initial antinodes
  foreach (var (a, b) in combinations)
  {
    // Calculate the distance between them as a vector
    var distance = (X: b.X - a.X, Y: b.Y - a.Y);
    // Extrapolate this to the two antinodes
    var anode1 = (X: a.X - distance.X, Y: a.Y - distance.Y);
    var anode2 = (X: b.X + distance.X, Y: b.Y + distance.Y);
    // Bounds-check the antinodes before adding to the set
    if (anode1.X >= 0 && anode1.X < mapWidth && anode1.Y >= 0 && anode1.Y < mapHeight) antinodes.Add(anode1);
    if (anode2.X >= 0 && anode2.X < mapWidth && anode2.Y >= 0 && anode2.Y < mapHeight) antinodes.Add(anode2);
  }
  
  // Part 2: All resonant nodes that fit on the map
  foreach (var (a, b) in combinations)
  {
    var distance = (X: b.X - a.X, Y: b.Y - a.Y);
    
    // Start at B and follow the distance vector "down" until we are off the map.
    // We are starting at B intentionally so that antenna A is counted as one of the nodes.
    var x = b.X - distance.X;
    var y = b.Y - distance.Y;

    while (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
    {
      resonantNodes.Add((x, y));
      x -= distance.X;
      y -= distance.Y;
    }
    
    // Now go back to A and follow the distance vector the other direction until we are off the map.
    // Again, we're intentionally overlapping to make sure that antenna B is counted as a node.
    x = a.X + distance.X;
    y = a.Y + distance.Y;
    
    while (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
    {
      resonantNodes.Add((x, y));
      x += distance.X;
      y += distance.Y;
    }
  }
}

Console.WriteLine($"Part 1: {antinodes.Count}");
Console.WriteLine($"Part 2: {resonantNodes.Count}");

/*
Console.WriteLine("Part 2 Map");
foreach (var rnode in resonantNodes)
{
  Span<char> col = input[rnode.Y].ToCharArray();
  col[rnode.X] = '#';
  input[rnode.Y] = new string(col);
}
foreach (var line in input)
{
  Console.WriteLine(line);
}
*/