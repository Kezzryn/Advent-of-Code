namespace Synacor_Challenge
{
    internal class PuzzleSolutions
    {
        private const ushort USHORT_32767 = 32767;
        private const ushort MODULO = 32768;
        private const ushort USHORT_1 = 1;
        const ushort TARGET_VALUE = 6;
        private ushort regSeven;
        private readonly Dictionary<(ushort, ushort), ushort> memo = new();

      //  public static int RegToInt((ushort r0, ushort r1) r) => RegToInt(r.r0, r.r1);
      //  public static int RegToInt(ushort r0, ushort r1) => (r0 << 16) | (r1 & 0xffff);
      //  private static (ushort reg0, ushort reg1) IntToReg(int i) => ((ushort)(i >> 16), (ushort)i);
        private static ushort SubOne(ushort a) => (ushort)((a + USHORT_32767) % MODULO);
        private static ushort AddOne(ushort a) => (ushort)((a + USHORT_1) % MODULO);

        public void TeleporterSolver(Object? obj)
        {
            ushort start;
            ushort end;
            (start, end) = (Tuple<ushort, ushort>)obj!;

            ushort answer;

            for (ushort testValue = start; testValue <= end; testValue++)
            {
                memo.Clear();
                regSeven = testValue;
                answer = Ack(4, 1);

                if (answer == TARGET_VALUE)
                {
                    Console.WriteLine($"Found it! {testValue}");
                    return;
                }
                else
                {
                    Console.WriteLine($"Ack(4, 1, {testValue}) == {answer} is not {TARGET_VALUE}");
                }
                //                if ((testValue % 100) == 0) Console.WriteLine($"Testing: {testValue}");
            }
            Console.WriteLine($"Nothing in: {start} to {end}");
        }
        ushort Ack(ushort r0, ushort r1)
        {
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
        public static int OrbSolver()
        {
            return -1;
        }
    }
}
