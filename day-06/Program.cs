/*
 * Advent of Code 2024, Day 6
 */

var input = File.ReadAllLines("input.txt");

// Parse the input file into a 2D array of the map, taking special note of the guard's starting position
(int X, int Y) position = (-1, -1);
var direction = Direction.North;
var map = new MapState[input.Length, input[0].Length];

for (var y = 0; y < input.Length; y++)
for (var x = 0; x < input[y].Length; x++)
{
    var symbol = input[y][x];
    switch (symbol)
    {
        case '.':
            map[x, y] = MapState.Empty;
            break;
        case '#':
            map[x, y] = MapState.Obstacle;
            break;
        case '^':
            map[x, y] = MapState.Empty;
            position = (x, y);
            break;
    }
}

var loops = new HashSet<(int X, int Y)>();
var track = new Direction[map.GetLength(0), map.GetLength(1)];

// Part 1: Do the walk and then count how many tracked spaces aren't empty
while (true)
{
    track[position.X, position.Y] |= direction;
    
    var movement = GetMovement(direction);
    var nextPosition = (X: position.X + movement.X, Y: position.Y + movement.Y);
    
    // If we are looking at going out of the map bounds, that's the end of the loop
    if (nextPosition.X < 0 ||
        nextPosition.X >= map.GetLength(0) ||
        nextPosition.Y < 0 ||
        nextPosition.Y >= map.GetLength(1))
        break;
    
    var nextMapState = map[nextPosition.X, nextPosition.Y];
    
    // If the space ahead is not an obstacle, move into that space and mark it as visited
    if (nextMapState is not MapState.Obstacle)
    {
        // Part 2 isn't working yet -- still infinite loops sometimes
        //if (TestForLoop(position, direction)) loops.Add(nextPosition);
        
        position = nextPosition;
        track[position.X, position.Y] |= direction;
    }
    else
    {
        direction = TurnRight(direction);
    }
}

var visitedSpaces = track.Cast<Direction>().Count(t => t > 0);
Console.WriteLine($"Part 1: {visitedSpaces}");
Console.WriteLine($"Part 2: {loops.Count}");

return;

// Part 2: If turning right takes you to a track you've already walked, an obstacle in front of you will cause a loop
bool TestForLoop((int X, int Y) startingPosition, Direction startingDirection)
{
    var testPosition = (startingPosition.X, startingPosition.Y);
    var testDirection = TurnRight(startingDirection);
    
    while (true)
    {
        var movement = GetMovement(testDirection);
        var nextPosition = (X: testPosition.X + movement.X, Y: testPosition.Y + movement.Y);
        
        // If we are looking at going out of the map bounds, that's the end of the walk
        if (nextPosition.X < 0 ||
            nextPosition.X >= map.GetLength(0) ||
            nextPosition.Y < 0 ||
            nextPosition.Y >= map.GetLength(1))
            return false;
        
        var nextMapState = map[nextPosition.X, nextPosition.Y];
        
        if (nextMapState is not MapState.Obstacle)
        {
            testPosition = nextPosition;
            
            var currentTrack = track[testPosition.X, testPosition.Y];
            // If we have previously been at this location, in this direction, it's a loop
            if ((currentTrack & testDirection) > 0) return true;
        }
        else
        {
            testDirection = TurnRight(testDirection);
        }
    }
}

(int X, int Y) GetMovement(Direction dir) => dir switch
{
    Direction.North => (0, -1),
    Direction.East => (1, 0),
    Direction.South => (0, 1),
    Direction.West => (-1, 0),
    _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
};

Direction TurnRight(Direction dir) => dir switch
{
    Direction.North => Direction.East,
    Direction.East => Direction.South,
    Direction.South => Direction.West,
    Direction.West => Direction.North,
    _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
};

[Flags]
enum Direction
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8,
}

enum MapState
{
    Empty,
    Obstacle,
}