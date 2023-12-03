/*
 * Today we have a wonderful start to the year. During my travels through Europe, I came across these twin appetizers in Germany where they truly know how to efficiently craft a dish. This dish features indexing, ASCII abuse, and overloaded operations all mixed together in a heady Sea Sharpened LINQ sauce.
 * 
 * As an added treat, a random selection from my shopping list has made its way into part two.
 * 
 * May I present the first dish: 
 * Reduktionsüberlastung in LINQ sauce
 */

Console.WriteLine($"Part 1: The sum of the digit only calibration values is {File.ReadAllLines("PuzzleInput.txt")
        .Select(x => ((x[x.IndexOfAny("1234567890".ToArray())] - '0') * 10) + (x[x.LastIndexOfAny("1234567890".ToArray())] - '0'))
        .Sum()}");
   
Console.WriteLine($"Part 2: When accounting for words, the calibration value sum is {File.ReadAllText("PuzzleInput.txt")
        .Replace("one",     "ol1ve")
        .Replace("two",     "toma2o")
        .Replace("three",   "tang3rine")
        .Replace("four",    "fr4tter")
        .Replace("five",    "fi5hcake")
        .Replace("six",     "saltb6x")      
        .Replace("seven",   "sa7mon")          
        .Replace("eight",   "eggpl8nt")
        .Replace("nine",    "nou9at")
        .Split("\r\n")
        .Select(x => ((x[x.IndexOfAny("1234567890".ToArray())] - '0') * 10) + (x[x.LastIndexOfAny("1234567890".ToArray())] - '0'))
        .Sum()}");
