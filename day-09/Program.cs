/*
 * Advent of Code 2024, Day 9
 */

var input = File.ReadAllText("input.txt");

// Cut the newline from the end and convert the digits to numbers -- '0' is ASCII 48
var map = input.TrimEnd().Select(c => c - 48).ToArray();


// Part 1: Defrag individual blocks
// We'll represent the disk as an array of ints, each int representing the File ID of the block

// The sum of the digits in the map represents the total storage space required
var disk = new int[map.Sum()];

var diskHead = 0;
foreach (var (index, size) in map.Index())
{
    // Even numbered indices contain file maps. Odd indices are empty spaces.
    // Use -1 to represent empty instead of null to prevent a gazillion boxings
    var fileId = index % 2 == 0 ? index / 2 : -1;
    for (var i = 0; i < size; i++)
    {
        disk[diskHead++] = fileId;
    }
}

// Defrag individual bits, then sumproduct the File IDs with their positions on disk
// Start placing on the first empty spot on the disk
var place = Array.FindIndex(disk, 0, x => x == -1);
// Start taking from the first non-empty spot on the disk
var take = Array.FindLastIndex(disk, x => x != -1);
while (place < take)
{
    disk[place] = disk[take];
    disk[take] = -1;
    
    // Find the next empty disk space to place at
    while (disk[place] != -1) place++;
    
    // Find the next non-empty disk space to take from
    while (disk[take] == -1) take--;
}

var part1Answer = disk.Index().Where(x => x.Item != -1).Sum(x => (long)(x.Item * x.Index));
Console.WriteLine($"Part 1: {part1Answer}");


// Part 2: Defrag whole files
// This time, we'll keep the map basically as-is, but include the file ID alongside its size
// Since we'll be doing a lot of re-arranging, a linked list seems like a prudent structure.

var allocTable = new LinkedList<(int Size, int FileId)>();
foreach (var (index, size) in map.Index())
{
    // We do not need to add size 0 spans with this model
    if (size == 0) continue;
    
    // Even indices contain files
    // Odd indices are empty space. Continuing to use File ID -1 to represent empties
    allocTable.AddLast((size, index % 2 == 0 ? index / 2 : -1));
}

var take2 = allocTable.Last;
while (take2 != allocTable.First)
{
    // Traverse backwards until you find a file
    while (take2!.Value.FileId == -1) take2 = take2.Previous;

    // Traverse forwards from the start until you find an empty space large enough to hold it.
    // Stop traversing if you reach the take node.
    var place2 = allocTable.First;
    while (place2 != take2 && (place2!.Value.FileId != -1 || place2.Value.Size < take2.Value.Size)) place2 = place2.Next;

    // If we advanced to the take node, we did not find an empty space to move this file. Continue with the next node back.
    if (place2 == take2)
    {
        take2 = take2.Previous;
        continue;
    }
    
    // Allocate the file ID at the place point we found
    allocTable.AddBefore(place2, (take2.Value.Size, take2.Value.FileId));
    // Then change the old alloation as empty space now
    take2.ValueRef.FileId = -1;
    
    // If the space we moved to was larger than the file, reduce the empty space
    if (place2.Value.Size > take2.Value.Size) place2.ValueRef.Size -= take2.Value.Size;
    // Otherwise, it was exactly the size of the file and that empty space node can be removed
    else allocTable.Remove(place2);

    // If the node before our now-empty take point is also empty, fold the nodes together
    if (take2.Previous?.Value.FileId == -1)
    {
        take2.ValueRef.Size += take2.Previous.Value.Size;
        allocTable.Remove(take2.Previous);
    }
    // Same with the node following
    if (take2.Next?.Value.FileId == -1)
    {
        take2.ValueRef.Size += take2.Next.Value.Size;
        allocTable.Remove(take2.Next);
    }
}

// The sumproduct part is a little more complicated this time since the position of each block isn't directly stored.
var part2Answer = allocTable.Aggregate((Sum: 0L, Index: 0),
    (count, node) =>
    {
        if (node.FileId == -1) return (count.Sum, count.Index + node.Size);

        return (count.Sum + Enumerable.Range(count.Index, node.Size).Sum(pos => (long)pos * node.FileId),
            count.Index + node.Size);
    });

Console.WriteLine($"Part 2: {part2Answer.Sum}");
