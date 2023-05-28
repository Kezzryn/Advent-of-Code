try
{
    // The moonspeak is strong
    // Implemtation build around this guide https://codeforces.com/blog/entry/72593
    // https://www.reddit.com/r/adventofcode/comments/eh1d6p/2019_day_22_part_2_tutorial/

    // MORE Information here: https://en.wikipedia.org/wiki/Linear_congruential_generator

    // Implementation checked against / borrowed from: https://github.com/yzhong52/AdventOfCode/blob/master/2019/src/y2019/day22.rs

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const long NUM_SHUFFLES_PART_1 = 1;
    const long NUM_SHUFFLES_PART_2 = 101741582076661;

    const long DECK_SIZE_PART_1 = 10007;
    const long DECK_SIZE_PART_2 = 119315717514047;

    //const int INST_CUT = 0;
    //const int INST_INC = 1;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    //List<(int inst, long value)> ReduceInstructions(string[] sourceText, long deckSize)
    //{
    //    // all this reduces things down to two instructions.
    //    List<(int inst, long value)> rv = new();

    //    foreach (string line in sourceText)
    //    {
    //        if (line == "deal into new stack") // This can be represented by a combo of other instructions.
    //        {
    //            rv.Add((INST_INC, deckSize - 1));
    //            rv.Add((INST_CUT, 1));
    //        }

    //        if (line.StartsWith("cut")) rv.Add((INST_CUT, int.Parse(line[4..])));
    //        if (line.StartsWith("deal with increment")) rv.Add((INST_INC, int.Parse(line[19..])));
    //    }

    //    bool isDone = false;
    //    while (!isDone)
    //    {
    //        isDone = true;
    //        for (int curr = 1; curr < rv.Count; curr++)
    //        {
    //            int prev = curr - 1;

    //            if (rv[curr].inst == rv[prev].inst)
    //            {
    //                // cut x -- cut y can be represented by cut x + y % decksize
    //                // increment x -- increment y can be represented by increment x * y % deckSize
    //                isDone = false;
    //                long newValue = INST_CUT == rv[curr].inst
    //                    ? rv[curr].value + rv[prev].value % deckSize
    //                    : rv[curr].value * rv[prev].value % deckSize;

    //                rv[prev] = (rv[curr].inst, newValue);
    //                rv.RemoveAt(curr);
    //                curr--;
    //            }
    //            if (rv[prev].inst == INST_CUT && rv[curr].inst == INST_INC)
    //            {
    //                // cut x -- increment y can be represented by increment y -- cut (x * y) % deckSize
    //                isDone = false;
    //                long newCutValue = rv[curr].value * rv[prev].value % deckSize;
    //                rv[prev] = rv[curr];
    //                rv[curr] = (INST_CUT, newCutValue);
    //            }
    //        }
    //    }
    //    return rv;
    //}

    //List<(int inst, long value)> instructions = ReduceInstructions(puzzleInput, DECK_SIZE_PART_1);

    //long DoArrayShuffle(long deckSize, long numShuffles)
    //{
    //    // Shuffle by simulation. Great for ... debugging? 
    //    long[] deckA = new long[deckSize];
    //    long[] deckB = new long[deckSize];

    //    ref long[] ptrSource = ref deckA;
    //    ref long[] ptrTarget = ref deckB;

    //    for (long i = 0; i < deckSize; i++)
    //    {
    //        ptrSource[i] = i;
    //    }

    //    for (long shuffles = 0; shuffles < numShuffles; shuffles++)
    //    {
    //        for (int inst = 0; inst < instructions.Count; inst++)
    //        {
    //            if (inst % 2 == 0)
    //            {
    //                ptrSource = ref deckA;
    //                ptrTarget = ref deckB;
    //            }
    //            else
    //            {
    //                ptrSource = ref deckB;
    //                ptrTarget = ref deckA;
    //            }

    //            switch (instructions[inst].inst)
    //            {
    //                //case INST_STACK:
    //                //    Array.Copy(ptrSource, ptrTarget, deckSize);
    //                //    Array.Reverse(ptrTarget);
    //                //    break;
    //                case INST_CUT:
    //                    long cutValue = instructions[inst].value;

    //                    long sourceIndex = cutValue >= 0 ? 0 : deckSize + cutValue;
    //                    long destIndex = cutValue >= 0 ? deckSize - cutValue : 0;
    //                    long length = Math.Abs(cutValue);

    //                    Array.Copy(ptrSource, sourceIndex, ptrTarget, destIndex, length);

    //                    sourceIndex = cutValue >= 0 ? deckSize - (deckSize - cutValue) : 0;
    //                    destIndex = cutValue >= 0 ? 0 : deckSize - (deckSize + cutValue);
    //                    length = deckSize - length;

    //                    Array.Copy(ptrSource, sourceIndex, ptrTarget, destIndex, length);

    //                    break;
    //                case INST_INC:
    //                    long incValue = instructions[inst].value;
    //                    for (long cardIndex = 0; cardIndex < deckSize; cardIndex++)
    //                    {
    //                        long newIndex = (cardIndex * incValue) % deckSize;
    //                        ptrTarget[newIndex] = ptrSource[cardIndex];
    //                    }
    //                    break;
    //            }
    //        }
    //    }
    //    return (numShuffles == NUM_SHUFFLES_PART_1) ? Array.IndexOf(ptrTarget, 2019) : ptrTarget[2020];
    //}

    (Int128 a, Int128 b) ComposeLCF((Int128 a, Int128 b) fx, (Int128 c, Int128 d) gx, Int128 modulo)
    {
        return ((fx.a * gx.c) % modulo, ((fx.b * gx.c) + gx.d) % modulo);
    }

    (Int128 a, Int128 b) ComposeLCF_POW((Int128 a, Int128 b) lcf, long numShuffles, long modulo)
    {
        (Int128 a, Int128 b) g = (1, 0);

        while (numShuffles > 0)
        {
            if (long.IsOddInteger(numShuffles))
            {
                g = ComposeLCF(g, lcf, modulo);
            }
            numShuffles /= 2;
            lcf = ComposeLCF(lcf, lcf, modulo);
        }
        return g;
    }

    (Int128, Int128) GetLCF(string[] shuffleInstructions, Int128 modulo)
    {
        // LCF function is f(x) = ax + b, a = 1, b = 0.
        (Int128, Int128) returnValue = (1, 0); // base LCF state.

        foreach (string line in shuffleInstructions)
        {
            // This can be represented by a combo of other instructions.
            if (line == "deal into new stack") 
            {
                returnValue = ComposeLCF(returnValue, (modulo - 1, 0), modulo);
                returnValue = ComposeLCF(returnValue, (1, -1), modulo);
            }

            if (line.StartsWith("cut"))
            {
                Int128 cutDepth = Int128.Parse(line[4..]);
                
                // normalize the cuts to be positive, or our reverse modulo will go weird. 
                if (cutDepth < 0) cutDepth = modulo - (-cutDepth) % modulo;

                returnValue = ComposeLCF(returnValue, (1, -cutDepth), modulo);
            }

            if (line.StartsWith("deal with increment"))
            {
                returnValue = ComposeLCF(returnValue, (Int128.Parse(line[19..]), 0), modulo);
            }
        }
        return returnValue;
    }

    static bool TryModInverse(Int128 number, long modulo, out Int128 result)
    {
        // from StackOverflow
        if (number < 1) throw new ArgumentOutOfRangeException(nameof(number));
        if (modulo < 2) throw new ArgumentOutOfRangeException(nameof(modulo));
        Int128 n = number;
        Int128 m = modulo, v = 0, d = 1;
        while (n > 0)
        {
            Int128 t = m / n, x = n;
            n = m % x;
            m = x;
            x = d;
            d = checked(v - t * x); // Just in case
            v = x;
        }
        result = v % modulo;
        if (result < 0) result += modulo;
        if (number * result % modulo == 1) return true;
        result = default;
        return false;
    }

    Int128 DoLCFShuffle(long deckSize, long numShuffles, Int128 cardIndex)
    {
        (Int128 lcfA, Int128 lcfB) = ComposeLCF_POW(GetLCF(puzzleInput, deckSize), numShuffles, deckSize);

        return ((lcfA * cardIndex) + lcfB) % deckSize;
    }

    (Int128 a, Int128 b) UnShuffle(Int128 lcfA, Int128 lcfB, long numShuffles, long modulo)
    {
        if (numShuffles == 1)
        {
            return (lcfA % modulo, lcfB % modulo);
        }
        else
        {
            if (numShuffles % 2 == 0)
            {
                (Int128 m2, Int128 c2) = UnShuffle(lcfA, lcfB, numShuffles / 2, modulo);
                Int128 final_m = (m2 * m2) % modulo;
                Int128 final_c = (m2 * c2 + c2) % modulo;
                return (final_m, final_c);
            }
            else
            {
                (Int128 m1, Int128 c1) = UnShuffle(lcfA, lcfB, numShuffles - 1, modulo);
                Int128 final_m = (lcfA * m1) % modulo;
                Int128 final_c = (lcfA * c1 + lcfB) % modulo;
                return (final_m, final_c);
            }
        }
    }

    Int128 UndoLCFShuffle( long numShuffles, Int128 cardIndex, long modulo)
    {
        (Int128 lcfA, Int128 lcfB) = GetLCF(puzzleInput, modulo);

        TryModInverse(lcfA, modulo, out Int128 invLCF_A);
        Int128 invLCF_B = (-invLCF_A * lcfB) % modulo;

        (Int128 unLCF_A, Int128 unLCF_B) = UnShuffle(invLCF_A, invLCF_B, numShuffles, modulo);

        return ((cardIndex * unLCF_A) + unLCF_B) % modulo;
    }

    Int128 part1Answer = DoLCFShuffle(DECK_SIZE_PART_1, NUM_SHUFFLES_PART_1, 2019);
    Int128 part2Answer = UndoLCFShuffle(NUM_SHUFFLES_PART_2, 2020, DECK_SIZE_PART_2);

    Console.WriteLine($"Part 1: At the end of the shuffle, 2019 is in the {part1Answer} position.");
    Console.WriteLine($"Part 2: With a massive deck, {part2Answer} appears in the 2020th position.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}