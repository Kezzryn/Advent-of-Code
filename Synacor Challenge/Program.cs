using Synacor_Challenge;
try
{
    const string CHALLENGE_BIN = "challenge.bin";
    //const string CHALLENGE_DUMP = @"..\..\..\challenge.dump";
   
    Synacor9000 syn9k = new();
        
    syn9k.Load(new(new FileStream(CHALLENGE_BIN, FileMode.Open, FileAccess.Read)));

    //Decompiler.DumpIt(new(new FileStream(CHALLENGE_BIN, FileMode.Open, FileAccess.Read)),new(new FileStream(CHALLENGE_DUMP, FileMode.Create, FileAccess.Write)));

    syn9k.Run();

}
catch (Exception e)
{
    Console.WriteLine(e);
}