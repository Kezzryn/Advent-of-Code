namespace AoC_2022_Day_25
{
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
                returnValue = $"{dictionary.GetChar(digit)}{returnValue}";

                inputValue /= dictionary.PowerOf;
                if (dictionary.GetOffset() > 0) inputValue += (digit - dictionary.GetOffset() > 0) ? 1 : 0; //carry check. 

            } while (inputValue > 0);

            return returnValue;
        }
    }
}
