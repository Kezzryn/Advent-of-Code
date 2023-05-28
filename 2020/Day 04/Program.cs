using System.Text.RegularExpressions;

static bool Between(int value, int a, int b)
{
    if (a < b) 
        return a <= value && value <= b;
    else
        return b <= value && value <= a;
}

try
{
    const string CRLF = "\r\n";

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF).ToList();

    int part1Answer = 0;
    int part2Answer = 0;
    foreach (string passport in puzzleInput)
    {
        var passportData = passport.Replace(CRLF, " ").Split(' ').ToDictionary(k => k.Split(':').First(), v => v.Split(':').Last());

        if (passportData.Count == 8 || passportData.Count == 7 && !passportData.ContainsKey("cid"))
        {
            part1Answer++;

            //byr(Birth Year) - four digits; at least 1920 and at most 2002.
            int byr = int.Parse(passportData["byr"]);
            if (!Between(byr, 1920, 2002)) continue;

            //iyr(Issue Year) - four digits; at least 2010 and at most 2020.
            int iyr = int.Parse(passportData["iyr"]);
            if (!Between(iyr, 2010, 2020)) continue;

            //eyr(Expiration Year) - four digits; at least 2020 and at most 2030.
            int eyr = int.Parse(passportData["eyr"]);
            if (!Between(eyr, 2020, 2030)) continue;

            //hgt(Height) - a number followed by either cm or in:
            var hgtResult = Regex.Match(passportData["hgt"], @"(\d+)((?:cm)|(?:in))");
            if (!hgtResult.Success) continue;

            int height = int.Parse(hgtResult.Groups[1].Value);
            if (hgtResult.Groups[2].Value == "cm")
            {
                //If cm, the number must be at least 150 and at most 193.
                if (!Between(height, 150, 193)) continue;
            }
            else
            {
                //If in, the number must be at least 59 and at most 76.
                if (!Between(height, 59, 76)) continue;
            }

            //hcl(Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            var hclResult = Regex.Match(passportData["hcl"], @"#[0-9a-f]{6}");
            if (!hclResult.Success) continue;

            //ecl(Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            List<string> eyeColor = new() { "amb","blu", "brn", "gry","grn","hzl","oth" };
            if (!eyeColor.Any(passportData["ecl"].Contains)) continue;

            //pid(Passport ID) - a nine - digit number, including leading zeroes.
            if (passportData["pid"].Length != 9 || !int.TryParse(passportData["pid"], out int _)) continue;
            
            //cid(Country ID) - ignored, missing or not.

            // got this far, it must be valid. 
            part2Answer++;
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answer} valid passports.");
    Console.WriteLine($"Part 2: With field validation, there are {part2Answer} valid passports.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}