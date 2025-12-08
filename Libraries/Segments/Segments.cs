using System.Numerics;

namespace BKH.Segments;

public struct Slice<T>(T start, T length) where T : INumber<T>
{
    public T Start { get; set; } = start;
    public T Length { get; set; } = length;
    public readonly T End { get { return Start + Length; } }

    //if a is contained within b. 
    public readonly bool Contains(Slice<T> a) => Contains(a, this);
    public static bool Contains(Slice<T> a, Slice<T> b) => a.Start >= b.Start && a.End <= b.End;
    public readonly bool Overlaps(Slice<T> a) => Overlaps(a, this);
    public static bool Overlaps(Slice<T> a, Slice<T> b) => (a.Start >= b.Start || a.End >= b.Start) && (a.Start <= b.End || a.End <= b.End);
    public readonly bool Adjacent(Slice<T> a) => Adjacent(a, this);
    public static bool Adjacent(Slice<T> a, Slice<T> b) => (a.End + T.One == b.Start || b.End + T.One == a.Start);
    public override readonly string ToString() => $"Start = {Start} End = {End}";
}

public class Segments<T> where T : INumber<T>
{
    private readonly SortedList<T, Slice<T>> _slices = [];

    public Segments() { }

    public void AddRange(T a, T b)
    {
        if (b < a) (a, b) = (b, a);
        AddSlice(new(a, b - a));
    }

    public IList<Slice<T>> GetSlices()
    {
        return _slices.Values;
    }

    public void AddSlice(Slice<T> newSlice)
    {
        if (!_slices.TryAdd(newSlice.Start, newSlice))
        {
            if (newSlice.Length <= _slices[newSlice.Start].Length) return; //same slice, or contained. Do nothing. 
            _slices[newSlice.Start] = newSlice;     //update with the new slice.
        }

        int segmentIndex = _slices.IndexOfKey(newSlice.Start);

        //get previous and next indexes (if available) to look for overlaps and adjaency.
        bool prevOverlap = false;
        bool nextOverlap = false;
        T prevKey = T.Zero;
        T nextKey = T.Zero;

        T mergedStart = newSlice.Start;
        T mergedEnd = newSlice.End;

        if (segmentIndex > 0)
        {
            Slice<T> prevSlice = _slices.GetValueAtIndex(segmentIndex - 1);
            prevKey = _slices.GetKeyAtIndex(segmentIndex - 1);

            if (prevSlice.Contains(newSlice))
            {
                _slices.Remove(newSlice.Start);
            }
            else if (newSlice.Overlaps(prevSlice) || newSlice.Adjacent(prevSlice))
            {
                prevOverlap = true;
                mergedStart = prevSlice.Start;
                if (prevSlice.End > mergedEnd) mergedEnd = prevSlice.End;
            }
        }

        while (segmentIndex < _slices.Count - 1)
        {
            Slice<T> nextSlice = _slices.GetValueAtIndex(segmentIndex + 1);
            nextKey = _slices.GetKeyAtIndex(segmentIndex + 1);
            if (newSlice.Contains(nextSlice))
            {
                if (nextSlice.End > mergedEnd) mergedEnd = nextSlice.End;
                _slices.Remove(nextSlice.Start);
            }
            else if (newSlice.Overlaps(nextSlice) || newSlice.Adjacent(nextSlice))
            {
                nextOverlap = true;
                if (nextSlice.End > mergedEnd) mergedEnd = nextSlice.End;
                break;
            }
            else
            {
                break;
            }
        }

        if (prevOverlap || nextOverlap)
        {
            if (prevOverlap) _slices.Remove(prevKey);
            if (nextOverlap) _slices.Remove(nextKey);
            _slices.Remove(newSlice.Start);

            _slices.Add(mergedStart, new Slice<T>(mergedStart, mergedEnd - mergedStart));
        }
    }

    public void PrintSlices()
    {
        foreach (Slice<T> slice in _slices.Values)
        {
            Console.WriteLine(slice);
        }
        Console.WriteLine();
    }
}