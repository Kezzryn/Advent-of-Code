static int getBigBit(UInt128 x, int pos) => (x & ((UInt128)1 << pos)) != 0 ? 1 : 0;

static long RunComboniator(string puzzleLine, bool doQuintuple = false)
{
    //Inintal data load.
    string[] split = puzzleLine.Split(' ');
    string maskPattern = String.Join('?', Enumerable.Repeat(split[0], doQuintuple ? 5 : 1));
    List<int> badSpringBlocks = String.Join(',', Enumerable.Repeat(split[1], doQuintuple ? 5 : 1)).Split(',').Select(int.Parse).Reverse().ToList(); // reverse to make string vs bitmask work. 
    badSpringBlocks.Add(0);// add on zero end tibit so make this the same length as the ranges list.

    //parse out the springs into giant bitmasks.
    string[] chunks = maskPattern.Chunk(64).Select(x => new string(x)).ToArray();

    UInt128 maskKnownBrokenSpring = Convert.ToUInt64(chunks[0].Replace('#', '1').Replace('?', '1').Replace('.', '0'), 2);
    UInt128 maskKnownGoodSpring = Convert.ToUInt64(chunks[0].Replace('#', '0').Replace('?', '1').Replace('.', '1'), 2);

    if (maskPattern.Length > 64)
    {
        maskKnownBrokenSpring <<= chunks[1].Length;
        maskKnownBrokenSpring |=Convert.ToUInt64(chunks[1].Replace('#', '1').Replace('?', '1').Replace('.', '0'), 2);
        maskKnownGoodSpring <<= chunks[1].Length;
        maskKnownGoodSpring |= Convert.ToUInt64(chunks[1].Replace('#', '0').Replace('?', '1').Replace('.', '1'), 2);
    }

    // the pattern is a repeating set of 1 to X good springs, followed by a known number of bad springs, with 0 or more good springs at the start and end of the pattern. Break it out and buld a set of ranges we can test on.
    int minStartBlanks = maskPattern.TakeWhile(x => x == '.').Count();
    int minEndBlanks = maskPattern.Reverse().TakeWhile(x => x == '.').Count();

    int goodSpringCount = maskPattern.Length - badSpringBlocks.Sum();
    int manditorySpaces = badSpringBlocks.Count - 2; // -1 for - bound, - 1 to account for the 0 we added. 
    int rangeMax = (goodSpringCount - manditorySpaces - minStartBlanks - minEndBlanks) + 1;

    List<List<int>> goodSpringRanges = new(); // remeber, load end to front to match bitmask inversion.

    goodSpringRanges.Add(Enumerable.Range(minEndBlanks, rangeMax).ToList()); //number of spaces at start, if any
    for (int i = 0; i < manditorySpaces; i++)
    {
        goodSpringRanges.Add(Enumerable.Range(1, rangeMax).ToList()); //variable number of spaces between the broken spring notes
    }
    goodSpringRanges.Add(Enumerable.Range(minStartBlanks, rangeMax).ToList()); //number of spaces at end, if any

    //Now pass it all through a priority queue with pitch perfect pruning. 

    bool isValid(int numGood, int numBroken, int maskStart)
    {
        // TODO: Replace with a proper bitmask check.
        for (int i = 0; i < numGood; i++)
        {
            if (getBigBit(maskKnownGoodSpring, maskStart + i) != 1) return false;
        }

        for (int i = 0; i < numBroken; i++)
        {
            if (getBigBit(maskKnownBrokenSpring, maskStart + numGood + i) != 1) return false;
        }

        return true;
    }

    Dictionary<(int, int), long> answerCount = new() { { (0, 0), 1 } }; //key is our queue value and priority.
    PriorityQueue<int, int> answersQueue = new();
    answersQueue.Enqueue(0, 0);

    long returnValue = 0;
    while (answersQueue.TryDequeue(out int existingSetSpringCount, out int badBlockIndex))
    {
        answerCount.Remove((existingSetSpringCount, badBlockIndex), out long currentComboCount);

        int numBadSprings = badSpringBlocks[badBlockIndex];
        foreach (int numGoodSprings in goodSpringRanges[badBlockIndex])
        {
            int newSetSpringCount = existingSetSpringCount + numGoodSprings + numBadSprings;
            if (newSetSpringCount > maskPattern.Length) continue;   // sanity check. potential combo is too large.

            if (isValid(numGoodSprings, numBadSprings, existingSetSpringCount))
            {
                if (badBlockIndex == goodSpringRanges.Count - 1) // can't go deeper, so check for scoring.
                {
                    if (newSetSpringCount == maskPattern.Length) returnValue += currentComboCount;
                }
                else
                {
                    // if we have nothing counted at the next level, then enqueue, otherwise we're duplicating states and that's capital B bad. 
                    // thank you to https://github.com/Stevie-O/aoc-public/blob/master/2023/day12/aoc-2023-day12-part2-norecursion-nomemo.linq#L70
                    // for the nudge toward this optimization.
                    if (!answerCount.TryGetValue((newSetSpringCount, badBlockIndex + 1), out long nextSpringCount))
                    {
                        nextSpringCount = 0;
                        answersQueue.Enqueue(newSetSpringCount, badBlockIndex + 1);
                    }
                    answerCount[(newSetSpringCount, badBlockIndex + 1)] = currentComboCount + nextSpringCount;
                }
            }
        }

    }
    return returnValue;
}

try
{
    bool DO_QUINTUPLE = true;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    long part1Answer = puzzleInput.Sum(s => RunComboniator(s));
    long part2Answer = puzzleInput.Sum(s => RunComboniator(s, DO_QUINTUPLE));

    Console.WriteLine($"Part 1: The springs have {part1Answer} possible arrangements.");
    Console.WriteLine($"Part 2: Unfolded records have {part2Answer} possible arrangements.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}