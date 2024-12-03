/*
 * Advent of Code 2024, Day 2
 */

// Read the input file into an array of reports, each being an arrays of integer levels
var input = File.ReadAllLines("input.txt");
var reports = input.Select(line => line.Split(" ").Select(int.Parse).ToArray()).ToArray();

// Part 1: Count safe reports
var reportSafety = reports.Select(IsSafeReport);
var safeCount = reportSafety.Count(r => r);
Console.WriteLine(safeCount);

// Part 2: Count safe reports allowing for one false reading
var dampenedReportSafety = reports.Select(IsSafeWithDampening);
var dampenedSafeCount = dampenedReportSafety.Count(r => r);
Console.WriteLine(dampenedSafeCount);

return;

// Part 1
bool IsSafeReport(IList<int> levels)
{
    if (levels.Count < 2) throw new ArgumentException("Need at least 2 levels");
    
    var isAscending = levels[1] > levels[0];
    
    // Start with the second reading and compare it to the previous one
    for (var i = 1; i < levels.Count; i++)
    {
        // Levels must be consistently ascending or descending
        if (levels[i] > levels[i - 1] != isAscending) return false;
        
        // Levels must change by at least 1 and no more than 3
        var diff = Math.Abs(levels[i] - levels[i - 1]);
        if (diff is < 1 or > 3) return false;
    }

    // If we're here, no issues were found
    return true;
}

// Part 2
bool IsSafeWithDampening(int[] levels)
{
    // If the report is safe without dampening, nothing further is needed
    if (IsSafeReport(levels)) return true;

    // Reuse an array for our report, with one fewer levels in it
    var shorterLevels = new int[levels.Length - 1];
    // Iterate through skipping each level
    for (var i = 0; i < levels.Length; i++)
    {
        // Fill the array with everything prior to the skipped element
        if (i > 0)
            Array.Copy(levels, 0, shorterLevels, 0, i);
        // Fill the array with everything following the skipped element
        if (i < levels.Length - 1)
            Array.Copy(levels, i + 1, shorterLevels, i, shorterLevels.Length - i);
        
        // If skipping this element made it safe, we're good
        if (IsSafeReport(shorterLevels)) return true;
    }

    // If we're here, no safe report was found
    return false;
}