namespace AoC_2022_Day_23
{
    class Log
    {

        private readonly StreamWriter sw;

        public Log(string logFile)
        {
            sw = new StreamWriter(logFile);
        }

        ~Log()
        {
            sw.Close();
        }

        public void LogLine(string line)
        {
            sw.WriteLine(line);
        }
    }
}
