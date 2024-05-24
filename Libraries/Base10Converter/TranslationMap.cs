namespace BKH.Base10Converter
{
    public class TranslationMap(string symbols, int offset = 0)
    {
        /// <summary>
        /// Default Bin, Oct, Hex and Snafu maps.
        /// </summary>
        public static readonly Dictionary<string, TranslationMap> Maps = new()
        {
            {"BIN", new TranslationMap("01")},
            {"OCT", new TranslationMap("01234567")},
            {"HEX", new TranslationMap("0123456789ABCDEF")},
            {"SNAFU", new TranslationMap("=-012", 2) }   //AoC 2022 Day 25.
        };

        //symbol chart, lowest symbol to highest. 
        private readonly string _symbolList = symbols;

        //symbol chart, rearranged to move a non-zero _zeroOffset around for easier manipulation. 
        private readonly string _symbolsOffset = symbols[offset..] + symbols[..offset];

        //used for SNAFU type number sets where digits can represent negative values in their column.
        //Marks the zero bound index of the 0 character in _symbolList
        private readonly int _zeroOffset = offset;

        /// <summary>
        /// Given a value, return the translated representation of that character.
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>Symbol translation of the value.</returns>
        public char GetChar(int val) => _symbolsOffset[val];

        /// <summary>
        /// Given a character, returns the base 10 representation of that character.
        /// </summary>
        /// <param name="chr">Character to translate.</param>
        /// <returns>Base 10 representation of a character.</returns>
        public int GetValue(char chr) => _symbolList.IndexOf(chr) - _zeroOffset;

        /// <summary>
        /// The index of the 0 item in the translation map, usually the first item.
        /// Used for translations where a symbol can represent a negative number.
        /// </summary>
        public int ZeroOffset => _zeroOffset;

        /// <summary>
        /// The base of the map.
        /// </summary>
        public int Base => _symbolList.Length;
    }
}
