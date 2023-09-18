using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int RULES = 0;
    const int MESSAGES = 1;
    const bool DO_PART2 = true;
    const string CRLF = "\r\n";

    int A_POS = -1;
    int B_POS = -1;


    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF).ToArray();
    List<string> messages = puzzleInput[MESSAGES].Split(CRLF).ToList();

    Dictionary<int, List<List<int>>> rawRules = new();

    List<string> r42 = new();
    List<string> r31 = new(); 

    foreach (string rule in puzzleInput[RULES].Split(CRLF))
    {
        var r = rule.Split(new char[] { ':', '|' }, StringSplitOptions.TrimEntries);
        var pos = int.Parse(r[0]);

        if (r[1] == "\"a\"")
        {
            A_POS = pos;
            continue;
        }

        if (r[1] == "\"b\"")
        {
            B_POS = pos;
            continue;
        }

        rawRules.Add(pos, new());
        for(int i = 1; i < r.Length; i++)
        {
            rawRules[pos].Add(r[i].Split(" ").Select(int.Parse).ToList());
        }
    }

    // investigte the data. 
    // root -> 42 42 31
    // need only calc nodes 42 and 31 individually
    // then sanity check starts with, ends with 
    // part two has repeating 2-x 42 nodes, followed by 1-x 31 nodes. 

    List<string> BuildSubset(int ruleNumber)
    {
        Queue<List<int>> queue = new();
        HashSet<string> returnValue = new();

        foreach(var v in rawRules[ruleNumber])
        {
            queue.Enqueue(new(v));
        }

        while (queue.TryDequeue(out List<int>? queuevalue))
        {
            bool isChanged = false;
            for (int i = 0; i < queuevalue.Count; i++)
            {
                if (rawRules.TryGetValue(queuevalue[i], out List<List<int>>? rule))
                {
                    queuevalue.RemoveAt(i);
                    queuevalue.InsertRange(i, rule.First());
                    queue.Enqueue(new(queuevalue));
                    if (rule.Count == 2)
                    {
                        queuevalue.RemoveRange(i, rule.First().Count);
                        queuevalue.InsertRange(i, rule.Last());
                        queue.Enqueue(new(queuevalue));
                    }
                    isChanged = true;
                    break;
                }
            }

            if (!isChanged)
            {
                StringBuilder sb = new();
                for (int i = 0; i < queuevalue.Count; i++)
                {
                    if (queuevalue[i] == A_POS) sb.Append('a');
                    if (queuevalue[i] == B_POS) sb.Append('b');
                }
                returnValue.Add(sb.ToString());
            }
        }
        return returnValue.ToList();
    }
    
    bool isValid(string message, bool doPart2 = false)
    {
        int len = r31.First().Length;
        //All entries must start with 42 and end with 31
        if (!r42.Contains(message[..len])) return false;
        if (!r31.Contains(message[^len..])) return false;

        if (doPart2)
        {
            //any number of leading 42s, with a maximum of num42s - 1 trailing 31s.
            bool isR42 = true;
            int num42s = 0;
            int num31s = 0;

            for (int i = 0; i < message.Length; i += len)
            {
                string slice = message[i..(len + i)];

                if (isR42) //still doing rule42 testing?
                {
                    isR42 = r42.Contains(slice);
                    if (isR42) num42s++; // hey we found another one. 
                }

                if (!isR42) //done with rule42 testing, now doing rule31. 
                { 
                    if (!r31.Contains(slice)) return false;
                    num31s++;
                    if (num31s > num42s) return false;
                }
            }
            return num31s <= num42s - 1;
        }
        else
        {
            //Part 1 must follow a 42 42 31 pattern.
            //Sanity check the overall length. 
            if (message.Length != len * 3) return false;

            //We've already passed the start and end, so test the middle. 
            if (r42.Contains(message[len..(2 * len)])) return true;
            return false;
        }
    }

    r42 = BuildSubset(42);
    r31 = BuildSubset(31);

    int part1Answer = messages.Count(x => isValid(x));
    int part2Answer = messages.Count(x => isValid(x, DO_PART2));

    Console.WriteLine($"Part 1: The number of matching messages is {part1Answer}.");
    Console.WriteLine($"Part 2: After a rules adjustment, the number of matching messages is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}