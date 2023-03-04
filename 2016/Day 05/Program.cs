using AoC_2016_Day_05;
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
    string hash = "";

    Cinematic.DoBoot();

    System.Timers.Timer myTimer = new(250);
    myTimer.Elapsed += Cinematic.DoTick;
    myTimer.Start();

    while (!(isDoneP1 && isDoneP2))
    {
        counter++;
        hash = GetHash(md5Hash, $"{secretKey}{counter}");

        if (hash.StartsWith("00000"))
        {
            if(!isDoneP1)
            {
                Globals.part1Answer += hash[5];
                isDoneP1 = Globals.part1Answer.Length >= 8;
            }

            if (!isDoneP2)
            {
                if (hash[5] >= '0' && hash[5] <= '7')
                {
                    int pos = int.Parse(hash[5].ToString());
                    if (Globals.part2Answer[pos] != '?') continue;

                    StringBuilder tempString = new(Globals.part2Answer);
                    tempString[pos] = hash[6];
                    Globals.part2Answer = tempString.ToString();

                    isDoneP2 = !Globals.part2Answer.Contains('?');
                }
            }
        }
    }

    myTimer.Stop();
    Cinematic.DoCinematic(Cinematic.GRAND_FINALE);
    
}
catch (Exception e)
{
    Console.WriteLine(e);
}