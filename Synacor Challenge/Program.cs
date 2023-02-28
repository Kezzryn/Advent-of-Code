using Synacor_Challenge;
try
{
    const string CHALLENGE_BIN = "challenge.bin";
   
    Synacor9000 syn9k = new();
        
    syn9k.Load(new(new FileStream(CHALLENGE_BIN, FileMode.Open, FileAccess.Read)));
    syn9k.Run();

}
catch (Exception e)
{
    Console.WriteLine(e);
}