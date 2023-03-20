using System.Collections;

namespace AoC_2016_Day_16
{
    internal static class BitArray_Extensions
    {
        public static BitArray Prepend(this BitArray current, BitArray before)
        {
            var bools = new bool[current.Count + before.Count];
            before.CopyTo(bools, 0);
            current.CopyTo(bools, before.Count);
            return new BitArray(bools);
        }

        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }

        public static BitArray Reverse(this BitArray current)
        {
            var bools = new bool[current.Count];

            for (var i = 0; i < current.Count; i++)
            {
                bools[i] = current.Get(current.Count - 1 - i);
            }

            return new BitArray(bools);
        }
    }
}
