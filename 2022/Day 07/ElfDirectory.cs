namespace AoC_2022_Day_7;
{
    internal class ElfDirectory
    {
        //the size is the sum of all the files in this directory and all sub directories. 
        //when we update this number, we recursivly update it in all parent nodes. 
        private long _size = 0;

        public ElfDirectory(string name, ElfDirectory? parent)
        {
            Name = name;
            ParentDir = parent;
        }

        public long Size
        {
            get
            {
                return _size;
            }
        }

        public string Name { get; }

        public ElfDirectory? ParentDir { get; set; }

        public List<ElfFile> Files { get; set; } = new List<ElfFile>();

        public List<ElfDirectory> SubDirs { get; set; } = new List<ElfDirectory>();

        public void AddFile(string fileData)
        {
            long size = long.Parse(fileData.Split(" ")[0]);
            string name = fileData.Split(" ")[1];

            Files.Add(new ElfFile(name, size));
            AddSize(size);
        }

        public void AddSubDir(ElfDirectory subDir)
        {
            SubDirs.Add(subDir);
        }

        private void AddSize(long size)
        {
            _size += size;
            ParentDir?.AddSize(size);
        }
    }
}
