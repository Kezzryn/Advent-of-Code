using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

ConcurrentDictionary<byte, byte[]> encodedHexBytes = [];
foreach(byte b in Enumerable.Range(0, byte.MaxValue + 1).Select(x => (byte)x)) // +1 to extend the range to the full 255, or it stops at 254, 'cause zero bound.
{
    encodedHexBytes.TryAdd(b, Encoding.UTF8.GetBytes(b.ToString("x2")));
}

string GetHashFunc(string key, int stretch)
{
    byte[] hashData = [];
    byte[] hashBytes = new byte[32];

    hashData = MD5.HashData(Encoding.UTF8.GetBytes(key));

    for (int i = 0; i < stretch; i++)
    {
        //Dictionary copy to array elements is faster than String.Join or Stringbuilder conversion. 
        for (int j = 0; j < hashData.GetUpperBound(0); j++)
        {
            if (!encodedHexBytes.TryGetValue(hashData[j], out byte[]? t)) throw new KeyNotFoundException($"Unable to find key {hashData[j]}");
            hashBytes[j * 2] = t[0];
            hashBytes[(j * 2) + 1] = t[1];
        }

        hashData = MD5.HashData(hashBytes);
    }

    return String.Join("", hashData.Select(x => x.ToString("x2")));
}

int CountKeys(string secret, int NUM_HASHES = -1)
{
    const int RANGE_SKIP = 5000;
    Dictionary<char, List<int>> potentialKeys = [];

    int counter = 0;
    int numKeysFound = 0;

    byte[] hashData = [];
    byte[] hashBytes = new byte[32];

    while (numKeysFound < 64)
    {
        //Fetch RANGE_SKIP number of hashes to process. AsParrallel() makes it twice as fast, and heats your room.
        List<(int, string)> hashes = Enumerable.Range(counter, RANGE_SKIP).AsParallel().Select(x => (x, GetHashFunc($"{secret}{x}", NUM_HASHES))).ToList();

        counter += RANGE_SKIP;

        foreach ((int hashIndex, string hash) in hashes)
        {
            Match matchFive = MatchFive().Match(hash);
            Match matchThree = MatchThree().Match(hash);

            if (matchFive.Success)
            {
                char key = matchFive.Groups[0].Value.First();
                if (potentialKeys.TryGetValue(key, out List<int>? potentialHashIndexes))
                {
                    foreach (int potentialHashIndex in potentialHashIndexes.Where(x => (hashIndex - x) <= 1000).OrderBy(x => x))
                    {
                        numKeysFound++;
                        potentialHashIndexes.Remove(potentialHashIndex);

                        if (numKeysFound >= 64) return potentialHashIndex;
                    }
                }
            }

            if (matchThree.Success)
            {
                char key = matchThree.Groups[0].Value.First();
                if (!potentialKeys.TryAdd(key, [hashIndex]))
                {
                    potentialKeys[key].Add(hashIndex);
                }
            }
        }
    }
    return -1;
}

try
{
    const int PART2_KEY_STRETCH = 2016;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string secretKey = File.ReadAllText(PUZZLE_INPUT);
        
    int part1Answer = CountKeys(secretKey);
    int part2Answer = CountKeys(secretKey, PART2_KEY_STRETCH);

    Console.WriteLine($"Part 1: The index that produces the 64th key is: {part1Answer}.");
    Console.WriteLine($"Part 2: After implementing key stretching, the index that produces the 64th key is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

partial class Program
{
    [GeneratedRegex(@"(.)\1{4}")]
    private static partial Regex MatchFive();
}

partial class Program
{
    [GeneratedRegex(@"(.)\1{2}")]
    private static partial Regex MatchThree();
}