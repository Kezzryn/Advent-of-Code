namespace AoC_2017_KnotHash;
using System.Text;

public static class KnotHash
{
    public static string Part1Answer2017Day10(string puzzleInput)
    {
        return TieKnot(puzzleInput, true);
    }

    public static string TieKnot(string puzzleInput, bool do2017Day10 = false)
    {
        const int LIST_LENGTH = 256;
        byte[] _sparseHash = new byte[LIST_LENGTH];
        List<int> dataList = [];
        int numLoops = do2017Day10 ? 1 : 64;

        int _cursor = 0;
        int _skipSize = 0;

        for (int i = 0; i < LIST_LENGTH; i++)
        {
            _sparseHash[i] = (byte)i;
        }

        if (do2017Day10)
        {
            dataList = puzzleInput.Split(',').Select(int.Parse).ToList();
        }
        else
        {
            StringBuilder sb = new();
            sb.Append(puzzleInput);
            sb.Append((char)17);
            sb.Append((char)31);
            sb.Append('I');
            sb.Append('/');
            sb.Append((char)23);
            dataList = sb.ToString().Select(x => (int)x).ToList();
        }

        for (int loops = 0; loops < numLoops; loops++)
        {
            foreach (int length in dataList)
            {
                if (length > LIST_LENGTH) continue;

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

        return do2017Day10 ? Convert.ToString(_sparseHash[0] * _sparseHash[1]) : Convert.ToHexString(denseHash);
    }

    public static string HashStringToBinary(string input)
    {
        string temp = TieKnot(input);

        StringBuilder sb = new();
        foreach (var v in Convert.FromHexString(temp))
        {
            sb.Append(Convert.ToString(v, 2).PadLeft(8, '0'));
        }
        return sb.ToString();
    }
}