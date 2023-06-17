try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";

    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF);

    string[] rules = puzzleInput[0].Split(CRLF);
    int[] myTicket = puzzleInput[1].Split(CRLF).Last().Split(',').Select(int.Parse).ToArray();
    int[][] nearbyTickets = puzzleInput[2].Split(CRLF).Skip(1).Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();

    Dictionary<string, TicketField> ticketRules = new();

    foreach (string rule in rules)
    {
        int indexOfColon = rule.IndexOf(':');

        int[] ranges = rule[(indexOfColon + 1)..].Replace('-', ' ').Replace("or", " ").Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        ticketRules.Add(rule[..indexOfColon], new(ranges[0], ranges[1], ranges[2], ranges[3]));
    }

    int part1Answer = 0;
    long part2Answer = 0;
    List<int[]> cleanTickets = new();

    foreach (int[] ticket in nearbyTickets)
    {
        bool isBadValue = true;
        for (int pos = 0; pos < ticket.Length; pos++)
        {
            isBadValue = true;
            foreach (TicketField rule in ticketRules.Values)
            {
                if (rule.InRange(ticket[pos])) 
                {
                    isBadValue = false;
                    break;
                }
            }

            if (isBadValue)
            {
                part1Answer += ticket[pos];
                break;
            }
        }

        if (!isBadValue) cleanTickets.Add(ticket);
    }

    // Part 2
    Dictionary<string, List<int>> answerKey = new();

    foreach(string ID in ticketRules.Keys)
    {
        answerKey.Add(ID, new());
    }
     
    cleanTickets.Add(myTicket);
    foreach((string ID, TicketField rule) in ticketRules)
    {
        for (int pos = 0; pos < ticketRules.Count; pos++)
        {
            bool allInRange = true;
            foreach(int[] ticket in cleanTickets)
            {
                if (!rule.InRange(ticket[pos])) 
                {
                    allInRange = false;
                    break;
                }
            }
            if (allInRange)
            {
                answerKey[ID].Add(pos);
            }
        }
    }

    // Filter down the answerKey.
    while (answerKey.Any(x => x.Value.Count > 1))
    {
        foreach (var kvp in answerKey.Where(x => x.Value.Count == 1))
        {
            foreach (var kvp2 in answerKey.Where(x => x.Value.Count > 1))
            {
                kvp2.Value.Remove(kvp.Value.FirstOrDefault(-1));
            }
        }
    }

    part2Answer = 1;

    foreach((string ID, List<int> pos) in answerKey.Where(x => x.Key.Contains("departure")))
    {
        part2Answer *= myTicket[pos.FirstOrDefault(0)];
    }

    Console.WriteLine();

    Console.WriteLine($"Part 1: The ticket error scanning rate is {part1Answer}.");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

internal class TicketField
{
    public int MinA { get; set; } = 0;
    public int MaxA { get; set; } = 0;
    public int MinB { get; set; } = 0;
    public int MaxB { get; set; } = 0;
    public TicketField(int minA, int maxA, int minB, int maxB)
    {
            MinA = minA;
            MaxA = maxA;
            MaxB = maxB;
            MinB = minB;
    }
    public bool InRange(int x)
    {
        return (x - MinA) * (MaxA - x) >= 0
            || (x - MinB) * (MaxB - x) >= 0;
    } 
}