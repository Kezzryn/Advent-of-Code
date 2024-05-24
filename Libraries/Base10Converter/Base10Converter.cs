namespace BKH.Base10Converter
{
    public static class Base10Converter
    {
        /// <summary>
        /// Convert to base 10.
        /// </summary>
        /// <param name="inputValue">Value to convert</param>
        /// <param name="currMap">Translation map</param>
        /// <returns>Converted value.</returns>
        public static long ToBase10(string inputValue, TranslationMap currMap)
        {
            long returnValue = 0;
            long power = 1;

            foreach (char c in inputValue.Reverse())
            {
                returnValue += power * currMap.GetValue(c);
                power *= currMap.Base;
            }

            return returnValue;
        }
        /// <summary>
        /// Convert from a base 10 value.
        /// </summary>
        /// <param name="inputValue">Value to convert</param>
        /// <param name="currMap">Translation map</param>
        /// <returns>Converted value</returns>
        public static string FromBase10(long inputValue, TranslationMap currMap)
        {
            string returnValue = "";

            do
            {
                int digit = (int)(inputValue % currMap.Base);
                returnValue = $"{currMap.GetChar(digit)}{returnValue}";

                inputValue /= currMap.Base;
                if (currMap.ZeroOffset > 0) inputValue += (digit - currMap.ZeroOffset > 0) ? 1 : 0; //carry check. 

            } while (inputValue > 0);

            return returnValue;
        }
    }
}
