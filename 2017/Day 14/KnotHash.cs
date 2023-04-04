using System.Text;
namespace AoC_2017_Day_14
{
    internal static class KnotHash
    {
        public static string HashString(string input)
        {
            const int LIST_LENGTH = 256;
            const int NUM_ROTATIONS = 64;
            byte[] _sparseHash = new byte[LIST_LENGTH];

            int _cursor = 0;
            int _skipSize = 0;

            for (int i = 0; i < LIST_LENGTH; i++)
            {
                _sparseHash[i] = (byte)i;
            }

            StringBuilder sb = new();
            sb.Append(input);
            sb.Append((char)17);
            sb.Append((char)31);
            sb.Append((char)73);
            sb.Append((char)47);
            sb.Append((char)23);
            input = sb.ToString();

            for (int rotation = 0; rotation < NUM_ROTATIONS; rotation++)
            {
                foreach (int length in input)
                {
                    if (_cursor + length <= LIST_LENGTH)
                    {
                        Array.Reverse(_sparseHash, _cursor, length);
                    }
                    else
                    {
                        int i = _cursor;
                        int j = _cursor + length - 1;
                        while (i < j)
                        {
                            (_sparseHash[j % LIST_LENGTH], _sparseHash[i % LIST_LENGTH]) = (_sparseHash[i % LIST_LENGTH], _sparseHash[j % LIST_LENGTH]);
                            i++;
                            j--;
                        }
                    }

                    _cursor = (_cursor + length + _skipSize) % LIST_LENGTH;
                    _skipSize++;
                }
            }

            byte[] denseHash = new byte[16];

            for (int i = 0; i < LIST_LENGTH; i++)
            {
                int target = (i / 16) % 16;
                denseHash[target] ^= _sparseHash[i];
            }

            return Convert.ToHexString(denseHash).ToLower();
        } 

        public static string HashStringToBinary(string input)
        {
            string temp = HashString(input);
            
            StringBuilder sb = new();
            foreach(var v in Convert.FromHexString(temp))
            {
                sb.Append(Convert.ToString(v, 2).PadLeft(8,'0'));
            }
            return sb.ToString();
        }
    }
}
