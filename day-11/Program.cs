/*
 * Advent of Code 2024, Day 11
 */

var input = File.ReadAllText("input.txt").Split(' ').Select(long.Parse);

var stones = new LinkedList<long>();
foreach (var stone in input) stones.AddLast(stone);

foreach (var blink in Enumerable.Range(1, 75))
{
    var stone = stones.First;
    while (stone != null)
    {
        // If the stone has a 0, replace it with a 1
        if (stone.Value == 0)
        {
            stone.ValueRef = 1;
            stone = stone.Next;
        }
        // If the stone has an even number of digits, split it into two halves of that number
        else if (stone.Value.ToString().Length % 2 == 0)
        {
            var digits = stone.Value.ToString();
            var left = long.Parse(digits[..(digits.Length / 2)]);
            var right = long.Parse(digits[(digits.Length / 2)..]);
            var leftStone = stones.AddAfter(stone, left);
            var rightStone = stones.AddAfter(leftStone, right);
            stones.Remove(stone);
            stone = rightStone.Next;
        }
        else
        {
            stone.ValueRef = stone.Value * 2024;
            stone = stone.Next;
        }
    }
    
    Console.WriteLine($"After {blink}: {stones.Count}");
}
