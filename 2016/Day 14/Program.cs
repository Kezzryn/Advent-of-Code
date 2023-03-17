using System.Diagnostics;
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

    string secretKey = "abc"; //"File.ReadAllText(PUZZLE_INPUT);

    using MD5 md5Hash = MD5.Create();

    bool isDoneP1 = false;
    bool isDoneP2 = false;

    long counter = 0;
    long numKeysFound = 0;
    long part1Answer = 0;
    long part2Answer = 0;
    string hash = "";
    char key;

    Dictionary<char, List<long>> potentialKeys = new();

    while (!isDoneP1)
    {
        counter++;
        hash = GetHash(md5Hash, $"{secretKey}{counter}");
        
        var matchFive = Regex.Match(hash, @"(.)\1{4}");
        var matchThree = Regex.Match(hash, @"(.)\1{2}");


        if (matchFive.Success)
        {
            key = matchFive.Groups[0].Value.First();
            if (potentialKeys.TryGetValue(key, out List<long>? value))
            {
                var res = value.Where(x => (counter - x) <= 1000).OrderBy(x => x).FirstOrDefault(-1);

                if (res != -1 )
                {
                    Console.WriteLine($"Found match at {counter} {key} {res}");
                    numKeysFound++;
                    part1Answer = res;
                    value.Remove(res);
                }
            }
        }
        
        if (matchThree.Success)
        {
                
            key = matchThree.Groups[0].Value.First();
            if (!potentialKeys.TryAdd(key, new(){counter}))
            {
                potentialKeys[key].Add(counter);
            }
            Console.WriteLine($"Recording Triple {key} {counter} {hash}");
        }

        if (counter >= 1000 ) isDoneP1 = true;
        if (numKeysFound >= 64) isDoneP1 = true;
    }

    Console.WriteLine($"Part 1: The index that produces the 64th key is: {part1Answer}.");
    Console.WriteLine($"Part2: The first six zero AdventCoin hash is at {part2Answer}.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}