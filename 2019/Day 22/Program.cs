try
{
    // MORE Information here: https://en.wikipedia.org/wiki/Linear_congruential_generator

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const long NUM_SHUFFLES_PART_1 = 1;
    const long NUM_SHUFFLES_PART_2 = 101741582076661;

    const long DECK_SIZE_PART_1 = 10007;
    const long DECK_SIZE_PART_2 = 119315717514047;

    const int INST_STACK = 0;
    const int INST_CUT = 1;
    const int INST_INC = 2;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
    List<(int inst, long value)> instructions = new();

    foreach (string line in puzzleInput)
    {
        if (line == "deal into new stack") instructions.Add((INST_STACK, 0));
        if (line.StartsWith("cut")) instructions.Add((INST_CUT, int.Parse(line[4..])));
        if (line.StartsWith("deal with increment")) instructions.Add((INST_INC, int.Parse(line[19..])));
    }

    // https://github.com/kage23/advent-of-code/blob/main/src/2019/Day22.tsx
    // reduce instruction set
    /*
     * 
     * const convertToLCF = (instruction: string): [bigint, bigint] => {
  if (instruction.startsWith('deal with')) {
    const increment: bigint = BigInt(parseInt(instruction.split('increment ')[1]))
    return [increment, BigInt(0)]
  } else if (instruction.startsWith('deal into')) {
    return [BigInt(-1), BigInt(-1)]
  } else if (instruction.startsWith('cut')) {
    const cutBy = parseInt(instruction.slice(4))
    return [BigInt(1), BigInt(cutBy * -1)]
  }

  return [BigInt(NaN), BigInt(NaN)]
}
     * const simplifyInstructions = (instructions: string[], deckSize: bigint): [bigint, bigint] => {
          const originalLCFs = instructions.map(convertToLCF)

          let newLCF = originalLCFs[0]
          for (let i = 1; i < originalLCFs.length; i++) {
            const nextLCF = originalLCFs[i]
            newLCF = [
              (BigInt(newLCF[0]) * BigInt(nextLCF[0])) % BigInt(deckSize),
              ((BigInt(newLCF[1]) * BigInt(nextLCF[0])) + BigInt(nextLCF[1])) % BigInt(deckSize)
            ]
          }

          return newLCF
        }
     */


    /*
     * I really like this computer science approach. Expanding it a bit more:

deal into new stack
---
deal with increment (count-1)
cut 1
Then we only need the rules for:

cut x
cut y
---
cut (x+y) % count

deal with increment x
deal with increment y
---
deal with increment (x*y) % count

cut x
deal with increment y
---
deal with increment y
cut (x*y) % count
     */

    // Shuffle by simulation 
    long DoArrayShuffle(long deckSize, long numShuffles)
    {
        long[] deckA = new long[deckSize];
        long[] deckB = new long[deckSize];

        ref long[] ptrSource = ref deckA;
        ref long[] ptrTarget = ref deckB;

        for (long i = 0; i < deckSize; i++)
        {
            ptrSource[i] = i;
        }

        for (long shuffles = 0; shuffles < numShuffles; shuffles++)
        {
            for (int inst = 0; inst < instructions.Count; inst++)
            {
                if (inst % 2 == 0)
                {
                    ptrSource = ref deckA;
                    ptrTarget = ref deckB;
                }
                else
                {
                    ptrSource = ref deckB;
                    ptrTarget = ref deckA;
                }

                switch (instructions[inst].inst)
                {
                    case INST_STACK:
                        Array.Copy(ptrSource, ptrTarget, deckSize);
                        Array.Reverse(ptrTarget);
                        break;
                    case INST_CUT:
                        long cutValue = instructions[inst].value;

                        long sourceIndex = cutValue >= 0 ? 0 : deckSize + cutValue;
                        long destIndex = cutValue >= 0 ? deckSize - cutValue : 0;
                        long length = Math.Abs(cutValue);

                        Array.Copy(ptrSource, sourceIndex, ptrTarget, destIndex, length);

                        sourceIndex = cutValue >= 0 ? deckSize - (deckSize - cutValue) : 0;
                        destIndex = cutValue >= 0 ? 0 : deckSize - (deckSize + cutValue);
                        length = deckSize - length;

                        Array.Copy(ptrSource, sourceIndex, ptrTarget, destIndex, length);

                        break;
                    case INST_INC:
                        long incValue = instructions[inst].value;
                        for (long cardIndex = 0; cardIndex < deckSize; cardIndex++)
                        {
                            long newIndex = (cardIndex * incValue) % deckSize;
                            ptrTarget[newIndex] = ptrSource[cardIndex];
                        }
                        break;
                }
            }
        }
        return (numShuffles == NUM_SHUFFLES_PART_1) ? Array.IndexOf(ptrTarget, 2019) : ptrTarget[2020];
    }

    // The moonspeak is strong
    // * https://www.reddit.com/r/adventofcode/comments/eh1d6p/2019_day_22_part_2_tutorial/
    // * https://codeforces.com/blog/entry/72593
    // 8 https://github.com/yzhong52/AdventOfCode/blob/master/2019/src/y2019/day22.rs
    long DoMathShuffle(long deckSize, long numShuffles, long cardIndex)
    {
        // part one, all we care about is where a particular card ends up after each transformation.
        foreach((int ist, long value) in instructions)
        {
            switch (ist)
            {
                case INST_INC:
                    cardIndex = (cardIndex * value) % deckSize;
                    break;
                case INST_CUT:
                    cardIndex = (deckSize + cardIndex - value) % deckSize;
                    break;
                case INST_STACK:
                    cardIndex = deckSize - 1 - cardIndex;
                    break;
            }
        }
        return cardIndex;
    }

   // fn shuffle_part2(shuffles: Vec<Shuffle>, original_position: usize, deck_size: usize, times: usize) -> i128 {
   //     let deck_size = deck_size as i128;

   //     let mut reversed = shuffles.clone();
   //     reversed.reverse();

   //     // After shuffling, result = multiplier * result + constant
   //     // Refer to `shuffle_part1` also for the computation of `multiplier` and `constant`
   //     let mut multiplier: i128 = 1;
   //     let mut constant: i128 = 0;
   //     for row in shuffles {
   //         match row {
   //             Shuffle::DealWithIncrement (increment) => {
   //                 multiplier *= increment as i128;
   //                 constant *= increment as i128;
   //             }
   //             Shuffle::DealNewDeck => {
   //                 multiplier = -multiplier;
   //                 constant = -1 - constant;
   //             }
   //             Shuffle::Cut (count) => {
   //                 constant = constant - count as i128;
   //             }
   //         }
   //         multiplier = multiplier % deck_size as i128;
   //         constant = constant % deck_size as i128;
   //     }

   //     // "When did you become and expert in modular arithmetic? "
   //     // https://www.reddit.com/r/adventofcode/comments/eeb40v/day_22_part_2/
   //     let inverse_multiplier = modinverse(multiplier, deck_size as i128).unwrap();
   //     let inverse_constant = (-inverse_multiplier * constant) % deck_size as i128;

   //     let(repeated_inverse_multiplier, repeated_inverse_constant) = repeat(
   //         inverse_multiplier,
   //         inverse_constant,
   //         times,
   //         deck_size as i128,
    
   //     );

   //     (original_position as i128* repeated_inverse_multiplier + repeated_inverse_constant) % deck_size
   //}


    long part1Answer = DoArrayShuffle(DECK_SIZE_PART_1, NUM_SHUFFLES_PART_1);
    long part2Answer = DoMathShuffle(DECK_SIZE_PART_1, NUM_SHUFFLES_PART_1, 2019);

    Console.WriteLine();
    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}