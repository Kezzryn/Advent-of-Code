﻿namespace AoC_2015_Day_24
{
    public static class EnumExtentions
    {
        // https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
        // This works on lists of lists. 
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[]
            {
                Enumerable.Empty<T>()
            };

            return sequences.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence.Except(accseq)  //added Except to suppress combos of the same number. 
                select accseq.Concat(new[] { item }));
        }
    }
}
