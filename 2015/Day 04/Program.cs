using System.Security.Cryptography;
using System.Text;

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

    string secretKey = File.ReadAllText(PUZZLE_INPUT);

    using MD5 md5Hash = MD5.Create();

    bool isDoneP1 = false;
    bool isDoneP2 = false;

    long counter = 0;
    long part1Answer = 0;
    long part2Answer = 0;
    string hash = "";

    while (!(isDoneP1 && isDoneP2))
    {
        counter++;
        hash = GetHash(md5Hash, $"{secretKey}{counter}");

        if ( !isDoneP1 )
        {
            if (hash.StartsWith("00000"))
            {
                part1Answer = counter;
                isDoneP1 = true;
            }
        }

        if (!isDoneP2)
        {
            if (hash.StartsWith("000000"))
            {
                part2Answer = counter;
                isDoneP2 = true;
            }
        }
    }

    Console.WriteLine($"Part1: The first five zero AdventCoin hash is at {part1Answer}.");
    Console.WriteLine($"Part2: The first six zero AdventCoin hash is at {part2Answer}.");

} 
catch (Exception e)
{
    Console.WriteLine(e);
}


