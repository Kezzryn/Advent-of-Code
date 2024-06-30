try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int part1Answer = 0;
    string part2Answer = "";
    foreach (string line in puzzleInput)
    {
        //aaaaa-bbb-z-y-x-123[abxyz]
        //break into our three bits on on the last dash and the '[' 
        int lastDash = line.LastIndexOf('-');
        int bracket = line.IndexOf('[');
        string roomName = line[..lastDash];
        int sectorID = int.Parse(line[(lastDash + 1)..bracket]);
        string checksum = line[(bracket + 1)..^1];

        string checksumTest = String.Join("", 
            roomName.Replace("-", "")
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .ThenBy(x => x.Key)
                .SelectMany(x => x)
                .Distinct()
                .Take(5));

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(line.PadRight(65));

        if (checksum == checksumTest)
        {
            part1Answer += sectorID;

            // modulo of the sector ID to get our offset. This is the same core logic where we increment and modulo to loop an array counter.
            // the key addition here is the use of 'a' to properly offset our solution.  
            string decryptedRoom = String.Join("", roomName.Select(c => (c == '-') ? ' ' : (char)(((c + (sectorID % 26) - 'a') % 26) + 'a')));
            Console.ForegroundColor = ConsoleColor.Green;
            
            if (decryptedRoom.Contains("north"))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                part2Answer = ($"\"{decryptedRoom}\" is {sectorID}.");
            }
            Console.Write($" {decryptedRoom}");
        }
        Console.WriteLine();
    }

    Console.ResetColor();
    Console.WriteLine($"Part 1: The sum of the sector IDs of the real rooms is {part1Answer}");
    Console.WriteLine($"Part 2: The sector ID of the target room is {part2Answer}");
    
}
catch (Exception e)
{
    Console.WriteLine(e);
}