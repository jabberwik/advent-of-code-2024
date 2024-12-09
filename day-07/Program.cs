/*
 * Advent of Code 2024, Day 7
 */

using System.Diagnostics;

var input = File.ReadAllLines("input.txt");

var part1Answer = 0L;
var part2Answer = 0L;

// We can check each line in parallel. Parse out the numbers and add the target if a solution is found.
Parallel.ForEach(input, line =>
{
    var sides = line.Split(':');
    var target = long.Parse(sides[0]);
    var operands = sides[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

    if (IsThereASolutionPart1(target, operands)) Interlocked.Add(ref part1Answer, target);
    if (IsThereASolutionPart2(target, operands)) Interlocked.Add(ref part2Answer, target);
});

Console.WriteLine(part1Answer);
Console.WriteLine(part2Answer);
return;

bool IsThereASolutionPart1(long target, long[] operands)
{
    var operators = new Operator[operands.Length - 1];
    var result = 0L;
    
    // Iterate through every combination of operators possible
    // Since the examples don't have more than ~12 operators (2^12 = 4096 iterations) it's not too bad this way.
    var iterations = 1 << operators.Length;
    for (var i = 0; i < iterations; i++)
    {
        for (var o = 0; o < operators.Length; o++)
        {
            operators[o] = (i & (1 << o)) != 0 ? Operator.Multiply : Operator.Add;
        }

        result = DoTheMath(operands, operators);
        if (result == target)
        {
            Console.WriteLine($"Target {target} achieved after {i + 1} iterations");
            return true;
        }
    }

    Console.WriteLine($"Target {target} not achieved after {iterations} iterations");
    return false;
}

bool IsThereASolutionPart2(long target, long[] operands)
{
    var operators = new Operator[operands.Length - 1];
    var result = 0L;
    
    // Similar approach to Part 1 here.
    // 3^12 is a little harder to swallow as the worst-case, but fingers crossed!
    var iterations = Math.Pow(3, operands.Length);
    for (var i = 0; i < iterations; i++)
    {
        // Convert number to base-3 representation
        var remainingValue = i;
        for (var j = 0; j < operators.Length; j++)
        {
            operators[j] = (Operator)(remainingValue % 3);
            remainingValue /= 3;
        }
        
        result = DoTheMath(operands, operators);
        if (result == target)
        {
            Console.WriteLine($"Target {target} achieved after {i + 1} iterations");
            return true;
        }
    }
    
    Console.WriteLine($"Target {target} not achieved after {iterations} iterations");
    return false;
}
    
long DoTheMath(long[] operands, Operator[] operators)
{
    Debug.Assert(operands.Length == operators.Length + 1);

    var result = operands[0];
    for (var index = 0; index < operators.Length; index++)
    {
        switch (operators[index])
        {
            case Operator.Add:
                result += operands[index + 1];
                break;
            case Operator.Multiply:
                result *= operands[index + 1];
                break;
            case Operator.Concat:
                result = Concat(result, operands[index + 1]);
                break;
        }
    }
    return result;
}

long Concat(long a, long b) => long.Parse($"{a}{b}");

enum Operator
{
    Add,
    Multiply,
    Concat,
}