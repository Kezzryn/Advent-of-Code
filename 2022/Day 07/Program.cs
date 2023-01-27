using AoC_2022_Day_7;

try
{
    const string ROOT_DIR_NAME = "root";

    ElfDirectory currentDir = new(ROOT_DIR_NAME, null);
    List<ElfDirectory> directories = new() { currentDir };

    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 7.txt"; ;
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);


    //load our lists. 
    for (int currLine = 0; currLine < puzzleInput.Length; currLine++)
    {
        string[] cmd = puzzleInput[currLine].Split(" ");

        switch (cmd[1])
        {
            case "cd":
                currentDir = cmd[2] switch
                {
                    "/" => directories.First(x => x.Name == ROOT_DIR_NAME),
                    ".." => currentDir.ParentDir ?? currentDir,
                    _ => currentDir.SubDirs.First(x => x.Name == cmd[2]),
                };
                break;
            case "ls":
                var filesAndDirs = puzzleInput.Skip(currLine + 1).TakeWhile(x => !x.StartsWith("$"));

                foreach (var entry in filesAndDirs)
                {
                    if (entry.StartsWith("dir"))
                    {
                        ElfDirectory subDir = new(entry.Split(" ")[1], currentDir);
                        currentDir.AddSubDir(subDir);
                        directories.Add(subDir);
                    }
                    else
                    {
                        currentDir.AddFile(entry);
                    }
                }
                currLine += filesAndDirs.Count();
                break;
        }
    }

    //Part 1. Return the sum of all directories where their size is less than or equal to 100,000. 
    long sumDirs = directories.Select(x => x.Size).Where(x => x <= 100000).Sum();

    Console.WriteLine($"Part1: Total sum <= 100,000: {sumDirs}");


    //Part 2. Given the total capacity and target free space, find the smallest directory we can delete to free up enough space.
    const long DEVICE_CAPACITY = 70000000;
    const long TARGET_FREE_SPACE = 30000000;
    long FREE_SPACE = DEVICE_CAPACITY - directories.First(x => x.Name == ROOT_DIR_NAME).Size;

    ElfDirectory? bestDir = directories
        .OrderByDescending(x => FREE_SPACE + x.Size >= TARGET_FREE_SPACE)  //boolean return into two groups of true/false.  Decending gives us TRUE group first.
        .ThenBy(x => x.Size) //organize each group by ascending
        .FirstOrDefault(); //return first hit. (if any) 

    Console.WriteLine($"Part2: best directory to delete is: {bestDir?.Name} with a size of {bestDir?.Size}.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}