using AoC_2022_Day_23;

try
{
    bool verbose = false;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string LOG_FILE = "DebugLog.txt";

    string[] inputFile = File.ReadAllLines(PUZZLE_INPUT);
    Log infoLog = new Log(LOG_FILE);

    List<Elf> elves = new List<Elf>();

    LinkedList<string> direction = new();
    LinkedListNode<string> startDirection;
    LinkedListNode<string> currentDirection;

    direction.AddFirst("north");
    direction.AddLast("south");
    direction.AddLast("west");
    direction.AddLast("east");

    startDirection = direction.First;

    Dictionary<string, string> proposedMoves = new();

    long numRounds = 0;

    void WriteSolution(long k)
    {
        long minX = elves.Min(a => a.X);
        long maxX = elves.Max(a => a.X);
        long minY = elves.Min(a => a.Y);
        long maxY = elves.Max(a => a.Y);

        StreamWriter sw = new($"C:\\Users\\bholmes\\Desktop\\AoC\\day23ElfOutput{k}BAD.txt");
        for (long y = minY; y <= maxY; y++)
        {
            for (long x = minX; x <= maxX; x++)
            {
                if (elves.Find(a => a.X == x && a.Y == y) != null) sw.Write('#'); else sw.Write('.');
            }
            sw.WriteLine($" ::{(maxY - y) + minY} ");
            if (y == maxY)
            {
                for (long x = minX; x <= maxX; x++)
                {
                    var t = x.ToString();
                    sw.Write($"{t[t.Length - 1]}");
                }

            }
        }
        sw.Close();
    }

    //load elves
    for (int y = 0; y < inputFile.Length; y++)
    {
        for (int x = 0; x < inputFile[y].Length; x++)
        {
            if (inputFile[y][x] == '#') elves.Add(new Elf($"x{x}y{inputFile.Length - y}", x, inputFile.Length - y));
        }
    }

    if (verbose) WriteSolution(0);
    bool moving = true;
    while (moving)
    {
        if (verbose) infoLog.LogLine($"ROUND {numRounds}");
        if (verbose) infoLog.LogLine($"=====");
        proposedMoves.Clear();

        foreach (Elf e in elves)
        {
            e.bounce = false;
            e.proposed_X = 0;
            e.proposed_Y = 0;
        }

        foreach (Elf e in elves)
        {
            if (verbose) infoLog.LogLine($"Elf is thinking {e}");
            if (elves.FindAll(x => x.Y >= e.Y - 1 && x.Y <= e.Y + 1 && x.X >= e.X - 1 && x.X <= e.X + 1).Count == 1) // we should be the only result.
            {
                if (verbose) infoLog.LogLine($"We're alone, do nothing.");
                continue; //nobody around us, do nothing.
            }

            currentDirection = startDirection;
            for (int d = 1; d <= currentDirection.List.Count; d++)
            {
                bool isElf = false;
                string moveKey = "";

                switch (currentDirection.Value)
                {
                    case "north":
                        isElf = elves.Find(rv => rv.Y == e.Y + 1 && rv.X >= e.X - 1 && rv.X <= e.X + 1) != null;
                        moveKey = $"{e.X}, {e.Y + 1}";
                        break;
                    case "south":
                        isElf = elves.Find(rv => rv.Y == e.Y - 1 && rv.X >= e.X - 1 && rv.X <= e.X + 1) != null;
                        moveKey = $"{e.X}, {e.Y - 1}";
                        break;
                    case "east":
                        isElf = elves.Find(rv => rv.X == e.X + 1 && rv.Y >= e.Y - 1 && rv.Y <= e.Y + 1) != null;
                        moveKey = $"{e.X + 1}, {e.Y}";
                        break;
                    case "west":
                        isElf = elves.Find(rv => rv.X == e.X - 1 && rv.Y >= e.Y - 1 && rv.Y <= e.Y + 1) != null;
                        moveKey = $"{e.X - 1}, {e.Y}";
                        break;
                } //end switch

                if (!isElf)
                {
                    if (verbose) infoLog.LogLine($"Nobody {currentDirection.Value}");
                    if (!proposedMoves.TryAdd(moveKey, e.ID))
                    {
                        if (verbose) infoLog.LogLine($"collision at {moveKey} bouncing self bouncing elf ID {proposedMoves[moveKey]}");
                        elves.Find(x => x.ID == proposedMoves[moveKey]).bounce = true;
                        e.bounce = true;
                    }
                    else
                    {
                        e.proposed_X = int.Parse(moveKey.Split(",")[0]);
                        e.proposed_Y = int.Parse(moveKey.Split(",")[1]);
                        if (verbose) infoLog.LogLine($"Elf {proposedMoves[moveKey]} proposing move to {e.proposed_X} {e.proposed_Y}");
                    }
                }
                else
                {
                    if (verbose) infoLog.LogLine($"Elves found {currentDirection.Value} ");
                }

                if (proposedMoves.ContainsValue(e.ID) || e.bounce)
                {
                    if (verbose) infoLog.LogLine($"Found an answer or a bounce, breaking to next elf");
                    break;  //we found a move, or we bounced off our proposed move. 
                }
                currentDirection = (currentDirection.Next == null) ? currentDirection.List.First : currentDirection.Next;
            } //end for
        } // end for each elf

        foreach (KeyValuePair<string, string> kvp in proposedMoves)
        {
            Elf e = elves.Find(x => kvp.Value == x.ID);
            if (e.bounce == false)
            {
                e.X = e.proposed_X;
                e.Y = e.proposed_Y;
            }
        }

        if (verbose) WriteSolution(numRounds);

        startDirection = (startDirection.Next == null) ? startDirection.List.First : startDirection.Next;

        if (proposedMoves.Count == 0) moving = false; //we are done. 

        numRounds++;
        if (numRounds == 10) Console.WriteLine($"Total Area at round 10: area:{((elves.Max(rv => rv.X) - elves.Min(rv => rv.X) + 1) * (elves.Max(rv => rv.Y) - elves.Min(rv => rv.Y) + 1)) - elves.Count}");
        if (numRounds % 100 == 0) moving = false; // Console.WriteLine($"Round {numRounds} movecount: {proposedMoves.Count}");
    }

    Console.WriteLine($"Number of rounds till we're all spread out: {numRounds}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
