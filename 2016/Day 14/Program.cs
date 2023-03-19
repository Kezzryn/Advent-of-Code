using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

static string GetHash(HashAlgorithm hashAlgorithm, string input)
{
    // Copied from https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash

    // Convert the input string to a byte array and compute the hash.
    byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

    // Create a new Stringbuilder to collect the bytes and create a string.
    var sBuilder = new StringBuilder();

    // Loop through each byte of the hashed data and format each one as a hexadecimal string.
    for (int i = 0; i < data.Length; i++)
    {
        sBuilder.Append(data[i].ToString("x2"));
    }

    // Return the hexadecimal string.
    return sBuilder.ToString();
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART2_KEY_STRETCH = 2016;

    string secretKey = File.ReadAllText(PUZZLE_INPUT);

    using MD5 md5Hash = MD5.Create();

    long CountKeys(int NUM_HASHES = -1)
    {
        Dictionary<char, List<long>> potentialKeys = new();

        string hash = "";
        char key;
        bool isDone = false;
        long counter = 0;
        long numKeysFound = 0;
        long answer = 0;

        while (!isDone)
        {
            counter++;
            hash = GetHash(md5Hash, $"{secretKey}{counter}");

            for (int i = 0; i < NUM_HASHES; i++)
            {
                hash = GetHash(md5Hash, hash);
            }

            var matchFive = Regex.Match(hash, @"(.)\1{4}");
            var matchThree = Regex.Match(hash, @"(.)\1{2}");


            if (matchFive.Success)
            {
                key = matchFive.Groups[0].Value.First();
                if (potentialKeys.TryGetValue(key, out List<long>? value))
                {
                    var res = value.Where(x => (counter - x) <= 1000).OrderBy(x => x);

                    foreach (var item in res)
                    {
                        numKeysFound++;
                        answer = item;
                        value.Remove(item);

                        if (numKeysFound >= 64) return answer;
                    }
                }
            }

            if (matchThree.Success)
            {
                key = matchThree.Groups[0].Value.First();
                if (!potentialKeys.TryAdd(key, new() { counter }))
                {
                    potentialKeys[key].Add(counter);
                }
            }

            if (numKeysFound >= 64) isDone = true;
        }
        return answer;
    }

    long part1Answer = CountKeys();
    Console.WriteLine($"Part 1: The index that produces the 64th key is: {part1Answer}.");

    long part2Answer = CountKeys(PART2_KEY_STRETCH);
    Console.WriteLine($"Part 2: After implementing key stretching, the index that produces the 64th key is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}