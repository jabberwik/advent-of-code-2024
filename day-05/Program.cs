/*
 * Advent of Code 2024, Day 5
 */

var input = File.ReadAllLines("input.txt");

// The first part of the file is the ordering rules. Read until you hit a blank line.
var line = 0;
var rules = new List<(int Before, int After)>();
while (input[line] != string.Empty)
{
    var rulePages = input[line].Split("|").Select(int.Parse).ToArray();
    rules.Add((rulePages[0], rulePages[1]));
    line++;
}
// Create two indexes for the ordering rules
var pagesBefore = rules.ToLookup(rule => rule.After, rule => rule.Before);
var pagesAfter = rules.ToLookup(rule => rule.Before, rule => rule.After);

// Part 1: Identify which page sequences follow the ordering rules
bool CheckPrintingOrder(int[] pages)
{
    for (var pageIndex = 0; pageIndex < pages.Length; pageIndex++)
    {
        var page = pages[pageIndex];
        
        // Make sure that none of the pages before this one are required to be after this one
        if (pageIndex > 0 && pagesAfter.Contains(page))
        {
            for (var checkIndex = 0; checkIndex < pageIndex; checkIndex++)
            {
                if (pagesAfter[page].Contains(pages[checkIndex]))
                {
                    return false;
                }
            }
        }
        
        // Make sure that none of the pages after this one are required to be before this one
        if (pageIndex < pages.Length - 1 && pagesBefore.Contains(page))
        {
            for (var checkIndex = pageIndex + 1; checkIndex < pages.Length; checkIndex++)
            {
                if (pagesBefore[page].Contains(pages[checkIndex])) return false;
            }
        }
    }
    
    return true;
}

// Part 2: Correct the incorrect printing orders
int[] CorrectPrintingOrder(int[] pages)
{
    Array.Sort(pages, (a, b) =>
    {
        if (pagesBefore.Contains(a) && pagesBefore[a].Contains(b)) return -1;
        if (pagesAfter.Contains(a) && pagesAfter[a].Contains(b)) return 1;
        return 0;
    });
    return pages;
}

// Check each printing order
var part1Answer = 0L;
var part2Answer = 0L;
for (line++; line < input.Length; line++)
{
    var pages = input[line].Split(",").Select(int.Parse).ToArray();
    if (CheckPrintingOrder(pages))
    {
        var middlePage = pages[pages.Length / 2];
        part1Answer += middlePage;
    }
    else
    {
        var correctedOrder = CorrectPrintingOrder(pages);
        var middlePage = correctedOrder[correctedOrder.Length / 2];
        part2Answer += middlePage;
    }
}
Console.WriteLine($"Part 1: {part1Answer}");
Console.WriteLine($"Part 2: {part2Answer}");
