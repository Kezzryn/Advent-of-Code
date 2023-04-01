namespace AoC_2017_Day_10
{
    internal class KnotHash
    {
        private const int LIST_LENGTH = 256;
        private readonly byte[] _sparseHash = new byte[LIST_LENGTH];

        private int _cursor = 0;
        private int _skipSize = 0;

        public KnotHash()
        {
            ResetList();
        }

        public void ResetList()
        {
            for (int i = 0; i < LIST_LENGTH; i++)
            {
                _sparseHash[i] = (byte)i;
            }

            _cursor = 0;
            _skipSize = 0;
        }

        public int Part1Answer() => _sparseHash[0] * _sparseHash[1];

        public string Part2Answer()
        {
            byte[] denseHash = new byte[16];

            for (int i = 0; i < LIST_LENGTH; i++)
            {
                int target = (i / 16) % 16;
                denseHash[target] ^= _sparseHash[i];
            }

            return Convert.ToHexString(denseHash);
        } 

        public void TieKnot(int length)
        {
            if (length > LIST_LENGTH) return ;

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
}
