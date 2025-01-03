﻿namespace BKH.EnumExtentions;

using System;
using System.Collections;
using System.Numerics;

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

    /// <summary>
    /// Finds the prime factors of a number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns>IEnumerable<long> prime factors.</returns>
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

    /// <summary>
    /// Takes a IEnumerable list and returns a IEnumerable of IEnumerable's 
    /// </summary>
    /// <param name="list">The list to be grouped</param>
    /// <returns>List of grouped lists.</returns>
    /// <exception cref="ArgumentException">Throws an argument exception for an unsorted list.</exception>
    public static IEnumerable<IEnumerable<int>> GroupConsecutive(this IEnumerable<int> list)
    {
        //https://stackoverflow.com/questions/48387544/split-a-listint-into-groups-of-consecutive-numbers
        if (list.Any())
        {
            var count = 1;
            var startNumber = list.First();
            int last = startNumber;

            foreach (var i in list.Skip(1))
            {
                if (i < last)
                {
                    throw new ArgumentException($"List is not sorted.", nameof(list));
                }
                if (i - last == 1)
                    count += 1;
                else
                {
                    yield return Enumerable.Range(startNumber, count);
                    startNumber = i;
                    count = 1;
                }
                last = i;
            }
            yield return Enumerable.Range(startNumber, count);
        }
    }
    

    /// <summary>
    /// Prepends one BitArray to another.
    /// </summary>
    /// <param name="baseArray">The base BitArray.</param>
    /// <param name="prepend">The BitArray to prepend.</param>
    /// <returns>new BitArray</returns>
    public static BitArray Prepend(this BitArray baseArray, BitArray prepend)
    {
        bool[] bools = new bool[baseArray.Count + prepend.Count];
        prepend.CopyTo(bools, 0);
        baseArray.CopyTo(bools, prepend.Count);
        return new BitArray(bools);
    }

    /// <summary>
    /// Append one BitArray to another.
    /// </summary>
    /// <param name="baseArray">The base BitArray</param>
    /// <param name="after">The BitArray to append.</param>
    /// <returns></returns>
    public static BitArray Append(this BitArray baseArray, BitArray after)
    {
        var bools = new bool[baseArray.Count + after.Count];
        baseArray.CopyTo(bools, 0);
        after.CopyTo(bools, baseArray.Count);
        return new BitArray(bools);
    }

    /// <summary>
    /// Reverses the order of the elements in a BitArray.
    /// </summary>
    /// <param name="current"></param>
    /// <returns>a new BitArray</returns>
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