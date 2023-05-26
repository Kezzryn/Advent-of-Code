using AoC_2019_IntcodeVM;

try
{
    const int NUM_NIC = 50;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();

    List<IntcodeVM> nicList = new();
    for (int i = 0; i < NUM_NIC; i++)
    {
        nicList.Add(new IntcodeVM(puzzleInput));
        nicList[i].SetInput(i);
    }

    Queue<(int a, long x, long y)> messageQueue = new();

    (int a, long x, long y) natPacket = (0,0,0);

    long part1Answer = -1;
    long part2Answer = -1;
    long prevNatPacketY = -1;

    bool isDone = false;

    while (!isDone)
    {
        // run each machine. Give it a -1 if it has no input. 
        foreach (IntcodeVM activeVM in nicList)
        {
            if (!activeVM.HasInput) activeVM.SetInput(-1);
            activeVM.Run();

            // gather any output the run has generated. 
            while(activeVM.HasOutput)
            {
                activeVM.GetOutput(out long a);
                activeVM.GetOutput(out long x);
                activeVM.GetOutput(out long y);
                messageQueue.Enqueue(((int)a,x,y));
            }
        }

        // dispurse the output. 
        while (messageQueue.TryDequeue(out (int addr, long x, long y) packet))
        {
            if (packet.addr == 255)
            {
                natPacket = packet;
                if (part1Answer == -1) part1Answer = natPacket.y;
            } 
            else
            {
                nicList[packet.addr].SetInput(packet.x);
                nicList[packet.addr].SetInput(packet.y);
            }
        }

        // Idle network check.
        if (nicList.All(x => x.CurrentState == State.Paused_For_Input && !x.HasInput))
        {
            nicList[0].SetInput(natPacket.x);
            nicList[0].SetInput(natPacket.y);

            if (prevNatPacketY == natPacket.y) part2Answer = natPacket.y;
            prevNatPacketY = natPacket.y;
        }

        if (part1Answer != -1 && part2Answer != -1) isDone = true;
    }

    Console.WriteLine($"Part 1: The first value sent to address 255 is: {part1Answer}");
    Console.WriteLine($"Part 2: The first value sent from the NAT twice is a row is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}