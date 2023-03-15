using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace Synacor_Challenge
{
    internal record OrbMazeState
    {
        public Point pos = new(0, 0);
        public int Steps = 0;
        public int Weight = 22;
        public List<string> Path = new();

        public OrbMazeState() { }

        public OrbMazeState(OrbMazeState omz)
        {
            Path = new();
            pos = omz.pos;
            Steps = omz.Steps;
            Weight = omz.Weight;
            foreach (string s in omz.Path) { Path.Add(s); }
        }
    }

    internal static class PuzzleSolutions
    {
        const int TARGET_VALUE = 6;

        public static async Task<int> Solve_Teleporter()
        {
            const int INTERVAL = 1000;
            const int MAX = 32767;
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken cancel_token = cancelTokenSource.Token;
            int answer = -1;
            bool isDone = false;

            List<Task<int>> ack = new();
            int regSeven = 1;
            while (!isDone)
            {
                Console.WriteLine($"Loading taskgroup: {regSeven}");
                for (int i = regSeven; i < (regSeven + INTERVAL) && i < MAX; i++)
                {
                    ack.Add(PAckAsync(i, cancel_token));
                }
                regSeven += INTERVAL;

                while (ack.Count > 0)
                {
                    Task<int> finishedTask = await Task.WhenAny(ack);
                    await finishedTask;
                    ack.Remove(finishedTask);
                    answer = finishedTask.Result;
                    if (answer == -1) continue;

                    cancelTokenSource.Cancel();

                    Thread.Sleep(1000); //give time for all the threads to shut down.
                    cancelTokenSource.Dispose();
                    break;
                }

                if (cancelTokenSource.IsCancellationRequested || regSeven >= MAX) isDone = true;
            }
            return answer;
        }

        private static Task<int> PAckAsync(int reg7, CancellationToken cancellationToken)
        {
            // This is a procedural Ackermann implementation. 
            // Copied verbatum from Andy Christianson at https://github.com/NiXXeD/
            // A solution that I wouldn't have arrived at, but is obvious in hindsight. 

            // r0 goes from 4 to 0.  r1 can be between 0 and 32768, so that's our max search space.
            int[,] cache = new int[5, 32769];

            //   (0, _) => AddOne(r1)
            for (int i = 0; i <= 32768; i++)
            {
                cache[0, i] = (i + 1) % 32768;
            }

            // fill in each row of our solution space based on date from the previous rows.
            for (int r0 = 1; r0 <= 4; r0++)
            {
                //  (_, 0) => Ack(SubOne(r0), regSeven)
                cache[r0, 0] = cache[r0 - 1, reg7];

                // This one took me a hot minute. The second part represents the state that only exists in the first few calls of the function. 
                // After that's out of the way, r1 increases to the max of 32768 before rolling over to 0, which we handle on the previous line.
                // but, because we're counting up and not down, we need 
                for (int r1 = 1; (r1 < 32768 && r0 < 4) || (r1 < 2 && r0 == 4); r1++)
                {
                    if (cancellationToken.IsCancellationRequested) return Task.FromCanceled<int>(cancellationToken);
                    //Ack(SubOne(r0), Ack(r0, SubOne(r1)))
                    cache[r0, r1] = cache[r0 - 1, cache[r0, r1 - 1]];
                }
            }

            //and echo back the input that gave us the correct result. 
            return Task.FromResult(cache[4, 1] == TARGET_VALUE ? reg7 : -1);
        }

        public static List<string> Solve_Orb()
        {
            const int TARGET_WEIGHT = 30;
            const int ORB_Times = 100;
            const int ORB_Plus = 101;
            const int ORB_Minus = 102;
            Point startPoint = new(0, 0);
            Point endPoint = new(3, 3);

            Dictionary<Point, int> orb_map = new()
            {
                { new(0,3), ORB_Times },{ new(1,3), 0 },        { new(2,3), ORB_Minus },    { new(3,3), 1 },
                { new(0,2), 4 },        { new(1,2), ORB_Times },{ new(2,2), 11 },           { new(3,2), ORB_Times },
                { new(0,1), ORB_Plus }, { new(1,1), 4 },        { new(2,1), ORB_Minus },    { new(3,1), 18 },
                { new(0,0), 0 },        { new(1,0), ORB_Minus },{ new(2,0), 9 },            { new(3,0), ORB_Times }
            };

            static int NewWeight(int oldWeight, int orbOp, int mazeValue)
            {
                return orbOp switch
                {
                    ORB_Times => oldWeight * mazeValue,
                    ORB_Plus => oldWeight + mazeValue,
                    ORB_Minus => oldWeight - mazeValue,
                    _ => throw new NotImplementedException()
                };
            }

            static char MapToSymbol(int symbol)
            {
                return symbol switch
                {
                    ORB_Times => '*',
                    ORB_Plus => '+',
                    ORB_Minus => '-',
                    _ => throw new NotImplementedException()
                };
            }

            bool ValidStep(Point step) => step != startPoint && orb_map.ContainsKey(step); 

            List<Size> steps = new()
            {
                new Size( 0, +1), // N
                new Size( 0, -1), // S
                new Size(+1,  0), // E
                new Size(-1,  0),  // W
            };
            
            OrbMazeState start = new();
            start.Path.Add("22");

            OrbMazeState solution = new()
            {
                Weight = 50,
                Steps = int.MaxValue
            };

            PriorityQueue<OrbMazeState, int> queue = new();
            queue.Enqueue(start, start.Steps);

            while (queue.Count > 0)
            {
                OrbMazeState current = queue.Dequeue();

                if (current.Weight == TARGET_WEIGHT && current.pos == endPoint && current.Steps < solution.Steps)
                {
                    solution = new(current);
                    continue;   //we have a solution! 
                }

                if (current.Weight > 100 || current.Weight < 0) continue; 
                if (current.Steps > 50 || current.Steps > solution.Steps) continue;

                foreach (Size stepOne in steps)
                {
                    Point testStepOne = current.pos + stepOne;
                    if (!ValidStep(testStepOne)) continue;
                    foreach (Size stepTwo in steps)
                    {
                        Point testStepTwo = testStepOne + stepTwo;
                        if (!ValidStep(testStepTwo)) continue;

                        OrbMazeState nextStep = new(current);
                        nextStep.Steps += 2;
                        nextStep.pos = testStepTwo;

                        nextStep.Weight = NewWeight(current.Weight, orb_map[testStepOne], orb_map[testStepTwo]);

                        nextStep.Path.Add($" {MapToSymbol(orb_map[testStepOne])}");
                        nextStep.Path.Add($" {orb_map[testStepTwo]}");

                        queue.Enqueue(nextStep, nextStep.Steps);
                    }
                }
            }

            return solution.Path;
        }

        private static void BADTeleporterSolver(ushort start, ushort end)
        {
            Console.WriteLine("DO NOT USE THIS.");
            // this works for small increments.  It needs lots of refinement if it's going to be used.
            for (ushort testValue = start; testValue <= end; testValue++)
            {
                //18 MB seems to be min stack space that allows for solving
                Thread tp = new(AckWrapper, 18000000);
                tp.Start(testValue);
                // tp.Join();
            }

            void AckWrapper(object? obj)
            {
                const ushort USHORT_32767 = 32767;
                const ushort MODULO = 32768;
                const ushort USHORT_1 = 1;
                const ushort TARGET_VALUE = 6;
                Dictionary<(ushort, ushort), ushort> memo = new();
                ushort regSeven = (ushort)obj!;

                ushort answer = Ack(4, 1);
                if (answer == TARGET_VALUE)
                {
                    Console.WriteLine($"***Match on {regSeven}.");
                }
                else
                {
                    // Console.WriteLine($"No match on {regSeven}. Result {answer}.");
                }

                ushort Ack(ushort r0, ushort r1)
                {
                    // This works, but required 18 MB of stack space to execute. 
                    // It can be run in its own thread, or the system binary can be edited during the build process. 
                    if (memo.TryGetValue((r0, r1), out ushort memoReg)) return memoReg;

                    // Thanks to Wolfgang Ziegler (https://github.com/z1c0) for the (0,_) notation. I did not know you could do that. 
                    ushort ret = (r0, r1) switch
                    {
                        (0, _) => AddOne(r1),
                        (_, 0) => Ack(SubOne(r0), regSeven),
                        _ => Ack(SubOne(r0), Ack(r0, SubOne(r1)))
                    };

                    memo.TryAdd((r0, r1), ret);
                    return memo[(r0, r1)];
                }
                static ushort SubOne(ushort a) => (ushort)((a + USHORT_32767) % MODULO);
                static ushort AddOne(ushort a) => (ushort)((a + USHORT_1) % MODULO);
            }

        }
    }
}