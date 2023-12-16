namespace AoC_2023_Day_14
{
    internal static class ArrayUtils
    {
        public static bool Compare<T>(this T[,] firstArray, T[,] secondArray)
        {
            //from https://github.com/mohammedsouleymane/AdventOfCode/blob/main/AdventOfCode/Utilities/Util.cs
            return firstArray.Rank == secondArray.Rank &&
                   Enumerable.Range(0, firstArray.Rank).All(dimension => firstArray.GetLength(dimension) == secondArray.GetLength(dimension)) &&
                   firstArray.Cast<T>().SequenceEqual(secondArray.Cast<T>());
        }

        public static IEnumerable<T> SliceRow<T>(this T[,] array, int row)
        {
            for (var i = 0; i < array.GetLength(0); i++) yield return array[i, row];
        }

        public static void PasteRow<T>(this T[,] array, int row, T[] values)
        {
            for (var i = 0; i < array.GetLength(0); i++) array[i, row] = values[i];
        }

        public static IEnumerable<T> SliceCol<T>(this T[,] array, int col)
        {
            for (var i = 0; i < array.GetLength(1); i++) yield return array[col, i];
        }

        public static void PasteCol<T>(this T[,] array, int col, T[] values)
        {
            for (var i = 0; i < array.GetLength(1); i++) array[col, i] = values[i];
        }
    }
}
