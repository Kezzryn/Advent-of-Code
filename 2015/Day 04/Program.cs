using System.Security.Cryptography;
using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string secretKey = File.ReadAllText(PUZZLE_INPUT);
    int counter = 0;

    using MD5 md5Hash = MD5.Create();

    int part1Answer = -1;
    int part2Answer = -1;
    byte[] hashData = [];

    while (part1Answer == -1 || part2Answer == -1)
    {
        counter++;
        hashData = MD5.HashData(Encoding.UTF8.GetBytes($"{secretKey}{counter}"));

        if (hashData[0] == 0 && hashData[1] == 0 && hashData[2] < 10)
        {
            if (part1Answer == -1) part1Answer = counter;
            if (part2Answer == -1 && hashData[2] == 0) part2Answer = counter;
        }
    }

    Console.WriteLine($"Part 1: The first five zero AdventCoin hash is at {part1Answer}.");
    Console.WriteLine($"Part 2: The first six zero AdventCoin hash is at {part2Answer}.");
} 
catch (Exception e)
{
    Console.WriteLine(e);
}


