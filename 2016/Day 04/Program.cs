try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int part1Answer = 0;
    foreach (string puzzle in puzzleInput)
    {
        //aaaaa-bbb-z-y-x-123[abxyz]
        //break into our three bits on on the last dash and the '[' 
        int lastDash = puzzle.LastIndexOf('-');
        int lastBracket = puzzle.LastIndexOf('[');
        string roomName = puzzle[..lastDash];
        int sectorID = int.Parse(puzzle[(lastDash + 1)..lastBracket]);
        string checksum = puzzle[(lastBracket + 1)..^1];

        string checksumTest = String.Join("", 
            roomName.Replace("-", "")
                .OrderBy(x => x)        //pre sort so when we re-sort by group things with the same group level come out alphabetical. 
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .SelectMany(x => x)
                .Distinct()
                .Take(5));

        if (checksum == checksumTest)
        {
            part1Answer += sectorID;

            // modulo of the sector ID to get our offset. This is the same core logic where we increment and modulo to loop an array counter.
            // the key addition here is the use of 'a' to properly offset our solution.  
            string decryptedRoom = String.Join("", roomName.Select(c => (c == '-') ? ' ' : (char)(((c + (sectorID % 26) - 'a') % 26) + 'a')));
            if (decryptedRoom.Contains("north")) Console.WriteLine($"Part 2: The sector ID of \"{decryptedRoom}\" is {sectorID}.");
        }
    }

    Console.WriteLine($"Part 1: The sum of the sector IDs of the real rooms is {part1Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}