namespace AoC_2022_Day_17
{
    internal class Jets
    {
        //returns a char from a string. Loops when hits the end.

        public const char Left = '<';
        public const char Right = '>';

        private string _jetDirections;
        private int _jetIndex = -1;

        public Jets(string input)
        {
            _jetDirections = input;
        }

        public int GetJetIndex() => _jetIndex;
        public char GetNextJet()
        {
            _jetIndex++;
            if (_jetIndex > _jetDirections.Length - 1) _jetIndex = 0;

            return _jetDirections[_jetIndex];
        }
    }
}
