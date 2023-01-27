namespace AoC_2022_Day_7
{
    internal class ElfFile
    {
        public ElfFile(string name, long size)
        {
            Name = name;
            Size = size;
        }

        public string Name { get; }
        public long Size { get; }
    }
}
