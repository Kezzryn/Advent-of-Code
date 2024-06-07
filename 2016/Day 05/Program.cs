using AoC_2016_Day_05;
using System.Security.Cryptography;
using System.Text;

try
{
//    const string PUZZLE_INPUT = "PuzzleInput.txt";
//    string secretKey = File.ReadAllText(PUZZLE_INPUT);

    using MD5 md5Hash = MD5.Create();

    bool isDoneP1 = false;
    bool isDoneP2 = false;

    long counter = 0;
    byte[] hashData = [];
    string hashString = "";

    Cinematic.DoBoot();

    System.Timers.Timer myTimer = new(250);
    myTimer.Elapsed += Cinematic.DoTick;
    myTimer.Start();

    while (!(isDoneP1 && isDoneP2))
    {
        counter++;
        hashData = MD5.HashData(Encoding.UTF8.GetBytes($"{Globals.secretKey}{counter}"));

        if (hashData[0] == 0 && hashData[1] == 0 && hashData[2] < 16) //first five characters start with 0.
        {
            hashString = String.Join("", hashData.Select(x => x.ToString("x2")));
            if (!isDoneP1)
            {
                Globals.part1Answer += hashString[5];
                isDoneP1 = Globals.part1Answer.Length >= 8;
            }

            if (!isDoneP2 && hashString[5] >= '0' && hashString[5] <= '7')
            {
                int pos = hashString[5] - 48;
                if (Globals.part2Answer[pos] != '?') continue;
                Globals.part2Answer[pos] = hashString[6];

                isDoneP2 = !Globals.part2Answer.Contains('?');
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