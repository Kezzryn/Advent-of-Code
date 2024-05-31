using System.Numerics;

namespace BKH.EnumExtentions
{
    public static class EnumExtentions
    {
        /// <summary>
        /// Cartesian Product
        /// Returns the product of two or more lists.
        /// </summary>
        /// <typeparam name="T">The sequences to permutate.</typeparam>
        /// <param name="sequences"></param>
        /// <returns>A new sequence of sequences with the permutated values.</returns>
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            // https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
            // This works on lists of lists. 
            IEnumerable<IEnumerable<T>> emptyProduct = [[]];

            return sequences.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence.Except(accseq)  //added Except to suppress combos of the same sequence. 
                select accseq.Concat([item]));
        }

        /// <summary>
        /// The cumulative sum of a sequence of numbers.
        /// </summary>
        /// <param name="sequence">The sequence to sum.</param>
        /// <returns>The cumulatively summed list.</returns>
        public static IEnumerable<T> CumulativeSum<T>(this IEnumerable<T> sequence)
            where T : INumber<T>
        {
            // Adapted from here: https://stackoverflow.com/questions/4823467/using-linq-to-find-the-cumulative-sum-of-an-array-of-numbers-in-c-sharp
            // And here https://stackoverflow.com/questions/32664/is-there-a-constraint-that-restricts-my-generic-method-to-numeric-types
            T sum = T.Zero;

            foreach (var item in sequence)
            {
                sum += item;
                yield return sum;
            }
        }

        public static IEnumerable<long> Factor(this long number)
            //where T : IBinaryInteger<long>
        {
            // From StackOverflow. 
            // https://stackoverflow.com/questions/239865/best-way-to-find-all-factors-of-a-given-number

            long max = (long)Math.Sqrt(number);  // Round down

            for (long factor = 1; factor <= max; ++factor) // Test from 1 to the square root, or the int below it, inclusive.
            {
                if (number % factor == 0)
                {
                    if (factor != number / factor) // Don't add the square root twice!  Thanks Jon
                        yield return number / factor;
                }
            }
        }
    }
}