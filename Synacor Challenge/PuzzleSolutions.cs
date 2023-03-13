namespace Synacor_Challenge
{
    internal static class PuzzleSolutions
    {
        const int TARGET_VALUE = 6;

        //  public static int RegToInt((ushort r0, ushort r1) r) => RegToInt(r.r0, r.r1);
        //  public static int RegToInt(ushort r0, ushort r1) => (r0 << 16) | (r1 & 0xffff);
        //  private static (ushort reg0, ushort reg1) IntToReg(int i) => ((ushort)(i >> 16), (ushort)i);

        public static async Task<int> Solve()
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
                for(int i = regSeven; i < (regSeven + INTERVAL) && i < MAX; i++)
                {
                    ack.Add(PAckAsync(i, cancel_token));
                }
                regSeven += INTERVAL;

                Console.WriteLine($"Processing taskgroup.");
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

        public static int OrbSolver()
        {
            return -1;
        }


        public static void TeleporterSolver(ushort start, ushort end)
        {
            for (ushort testValue = start; testValue <= end; testValue++)
            {
                //18 MB seems to be min stack space that allows for solving
                Thread tp = new(AckWrapper, 18000000);
                tp.Start(testValue);
                tp.Join();
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
                    Console.WriteLine($"***Match on {regSeven}. Result {answer}.");
                }
                else
                {
                    Console.WriteLine($"No match on {regSeven}. Result {answer}.");
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
