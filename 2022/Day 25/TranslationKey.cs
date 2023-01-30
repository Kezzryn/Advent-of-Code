namespace AoC_2022_Day_25
{
    internal class TranslationKey
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
            //Decimal to character conversion
            return _symbolsOffset[index];
        }

        public int GetValue(char index)
        {
            // SNAFU EG: 
            // symbol list is: = - 0 1 2  _zeroOffset is 2
            // values range from -2 to +2, so fetching '-' at array index of 1, we apply the offset to return it's value of -1. 
            return _symbolList.IndexOf(index) - _zeroOffset;
        }

        public int GetOffset()
        {
            return _zeroOffset;
        }

        public int PowerOf => _symbolList.Length;

    }
}
