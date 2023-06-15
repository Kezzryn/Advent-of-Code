try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";

    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF);

    string[] rules = puzzleInput[0].Split(CRLF);
    string myticket = puzzleInput[1].Split(CRLF).Last();
    string[] otherTickets = puzzleInput[2].Split(CRLF).Skip(1).ToArray();


    Dictionary<string, TicketField> ticketRules = new();

    foreach (string rule in rules)
    {
        int indexOfColon = rule.IndexOf(':');
        int[] ranges = rule[(indexOfColon + 1)..].Replace('-', ' ').Replace("or", " ").Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        // 48-399 or 420-967

        ticketRules.Add(rule[..indexOfColon], new(ranges[0], ranges[1], ranges[2], ranges[3]));
    }

    int part1Answer = 0;
    int part2Answer = 0;
    Dictionary<string, int> placements = new();

    foreach (string ticket in otherTickets)
    {
        //placements.Clear();
        int[] ticketValue = ticket.Split(',').Select(int.Parse).ToArray();

        
        for (int pos = 0; pos < ticketValue.Length; pos++)
        {

            bool noMatch = true;
            foreach ((string name, TicketField rule) in ticketRules)
            {
                if (rule.InRange(ticketValue[pos])) 
                {
                    noMatch = false;
                    if(!placements.TryAdd(name, 1)) placements[name]++;
                }
            }

            if (noMatch)
            {
                part1Answer += ticketValue[pos];
                break;
            }
        }

        if (!noMatch)
        {
            foreach ((string name, int value) in placements)
            {
                Console.Write($"{value} ");
            }
            Console.WriteLine();
        }
    }

    
    Console.WriteLine($"Part 1: The ticket error scanning rate is {part1Answer}.");
    Console.WriteLine($"Part 2: {part2Answer} {otherTickets.Length}");
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
        if (minA < maxA)
        {
            MaxA = maxA;
            MinA = minA;
        } 
        else
        {
            MaxA = minA;
            MinA = maxA;
        }

        if (minB < maxB)
        {
            MaxB = maxB;
            MinB = minB;
        }
        else
        {
            MaxB = minB;
            MinB = maxB;
        }
    }
    public bool InRange(int x)
    {
        return (x - MinA) * (MaxA - x) >= 0
            || (x - MinB) * (MaxB - x) >= 0;
    } 
}