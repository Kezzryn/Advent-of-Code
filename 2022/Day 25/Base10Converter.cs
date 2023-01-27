namespace AoC_2022_Day_25
{
    record TranslationKey
    {
        //symbol chart, lowest symbol to highest. 
        private readonly string _symbolList = "";

        //symbol chart, rearranged to move a non-zero _zeroOffset around for easier manipulation. 
        private readonly string _symbolsOffset = "";

        //used for SNAFU type number sets where digits can represent negative values in their column.
        //Marks the zero bound index of the 0 character in _symbolList
        readonly int _zeroOffset = 0;

        public TranslationKey(string symbols, int offset = 0)
        {
            _symbolList = symbols;

            // take the 0th to the offset, and paste it to the end of the string. 
            // eg: =-012 -> 012=-
            // this can result in an identical list to _symbolList in the case of a 0 offset.
            _symbolsOffset = symbols[offset..] + symbols[..offset];

            _zeroOffset = offset;
        }

        public char GetChar(int index)
        {
            //TODO error check bounds 
            return _symbolsOffset[index];
        }

        public int GetValue(char index)
        {
            //TODO error check return value
            return _symbolList.IndexOf(index) - _zeroOffset;
        }

        public int GetOffset()
        {
            return _zeroOffset;
        }
        public int PowerOf => _symbolList.Length;

    }
    internal class Base10Converter
    {
        Dictionary<string, TranslationKey> _dictionary = new();

        public Base10Converter()
        {
            _dictionary.Add("Bin", new TranslationKey("01"));
            _dictionary.Add("Oct", new TranslationKey("01234567"));
            _dictionary.Add("Hex", new TranslationKey("0123456789ABCDEF"));
            _dictionary.Add("Snafu", new TranslationKey("=-012", 2));
        }

        public bool AddDictionary(string name, TranslationKey newKeys)
        {
            return _dictionary.TryAdd(name, newKeys);
        }

        public long ConvertToBase10(string value, string dicName)
        {

            long returnValue = 0;
            long power = 1;
            if (!_dictionary.TryGetValue(dicName, out TranslationKey? dictionary)) throw new Exception($"Unable to find {dicName} dictionary");

            foreach (char c in value.Reverse())
            {
                returnValue += power * dictionary.GetValue(c);
                power *= dictionary.PowerOf;
            }

            return returnValue;
        }

        public string ConvertFromBase10(long inputValue, string dicName)
        {
            string returnValue = "";
            if (!_dictionary.TryGetValue(dicName, out TranslationKey? dictionary)) throw new Exception($"Unable to find {dicName} dictionary");
            do
            {
                int digit = (int)(inputValue % dictionary.PowerOf);
                Console.WriteLine($"int {digit} = (int)({inputValue} % {dictionary.PowerOf});");
                returnValue = $"{dictionary.GetChar(digit)}{returnValue}";

                inputValue /= dictionary.PowerOf;
                if (dictionary.GetOffset() > 0) inputValue += (digit - dictionary.GetOffset() > 0) ? 1 : 0; //carry check. 

            } while (inputValue > 0);

            return returnValue;
        }
    }
}
