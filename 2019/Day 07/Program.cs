using AoC_2019_IntcodeVM;
using System.Diagnostics;

try
{
    //const string PROMPT = "_> ";
    Stopwatch sw = Stopwatch.StartNew();
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(int.Parse).ToArray();

    IntcodeVM[] thrusters = new IntcodeVM[5];
    for (int i = 0; i < 5; i++)
    {
        thrusters[i] = new(puzzleInput);
    }


    var rangePart1 = from a in Enumerable.Range(0, 5)
                     from b in Enumerable.Range(0, 5)
                     from c in Enumerable.Range(0, 5)
                     from d in Enumerable.Range(0, 5)
                     from e in Enumerable.Range(0, 5)
                     where 
                     a != b && a != c && a != d && a != e
                     && b != c && b != d && b != e
                      && c != d && c != e
                     && d != e
                     select new List<int>() { a, b, c, d, e };

    var rangePart2 = from a in Enumerable.Range(5, 5)
                     from b in Enumerable.Range(5, 5)
                     from c in Enumerable.Range(5, 5)
                     from d in Enumerable.Range(5, 5)
                     from e in Enumerable.Range(5, 5)
                     where
                     a != b && a != c && a != d && a != e
                     && b != c && b != d && b != e
                     && c != d && c != e
                     && d != e
                     select new List<int>() { a, b, c, d, e };

    List<int> part1Answers = new();
    foreach (var set in rangePart1)
    {
        int output = 0; 
        for(int i = 0; i < 5; i++)
        {
            thrusters[i].Reset(puzzleInput);
            thrusters[i].SetInput(set[i]);
            thrusters[i].SetInput(output);
            thrusters[i].Run();
            if (thrusters[i].GetOutput(out int? vmOut))
            {
                output = vmOut ?? throw new NullReferenceException();
            }
        }
        part1Answers.Add(output);
    }


    List<int> part2Answers = new();
    foreach (var set in rangePart2)
    {
        for (int i = 0; i < 5; i++)
        {
            thrusters[i].Reset(puzzleInput);
            thrusters[i].SetInput(set[i]);
        }
        State[] thrusterState = new State[5];

        int output = 0;
        bool isDone = false;
        while (!isDone)
        {
            for (int i = 0; i < 5; i++)
            {
                thrusters[i].SetInput(output);
                if (thrusterState[i] != State.Halted) thrusterState[i] = thrusters[i].Run();
                if (thrusters[i].GetOutput(out int? vmOut))
                {
                    output = vmOut ?? throw new NullReferenceException();
                }
            }

            isDone = thrusterState[0] == State.Halted;
            for(int i = 0; i < 5; i++)
            {
                isDone = isDone && thrusterState[i] == State.Halted;
            }
        }

        part2Answers.Add(output);
    }


    //bool isDone = false;
    //bool isRunning = false;
    //while (!isDone)
    //{
    //    Console.Write(PROMPT);
    //    string? readLine = Console.ReadLine();
    //    if (readLine == null) continue;

    //    switch (readLine.ToLower())
    //    {
    //        case "run":
    //            isRunning = true;
    //            break;
    //        case "reset":
    //            vm.Reset(puzzleInput);
    //            break;
    //        case "exit":
    //            isRunning = false;
    //            isDone = true;
    //            break;
    //        default:
    //            if (int.TryParse(readLine, out int output))
    //            {
    //                vm.SetInput(output);
    //            }
    //            else
    //            {
    //                Console.WriteLine($"Unable to parse {readLine}");
    //            }
    //            break;
    //    }

    //    if (isRunning)
    //    {
    //        isRunning = vm.Run() != State.Halted;

    //        while (vm.GetOutput(out int? output))
    //        {
    //            Console.WriteLine(output);
    //        }

    //        if (!isRunning) Console.WriteLine("Program halted.");
    //    }
    //}
    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds);
    Console.WriteLine($"Part 1: The highest signal that can be sent to the thrusters is {part1Answers.Max()}.");
    Console.WriteLine($"Part 2: With a feedback loop a signal of {part2Answers.Max()} can be sent to the thrusters.");
    
}
catch (Exception e)
{
    Console.WriteLine(e);
}