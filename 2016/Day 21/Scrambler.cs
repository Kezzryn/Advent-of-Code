namespace AoC_2016_Day_21
{
    internal class Scrambler
    {
        private enum Ops
        {
            Move,
            Rev,
            RotL,
            RotPos,
            RotPosRev,
            RotR,
            SwapPos,
            SwapLet
        };

        private readonly List<(Ops, int, int)> _instructionSet = [];
        private List<int> _scrambleText = [];

        public Scrambler(string[] instructions, bool doReversed = false)
        {
            IEnumerable<string> loadInstructions = doReversed ? instructions.Reverse() : instructions;

            foreach (string instruction in loadInstructions)
            {
                _instructionSet.Add(ParseInstruction(instruction, doReversed));
            }
        }

        public override string ToString() => new string(_scrambleText.Select(x => (char)x).ToArray()) ?? "";

        private void SwapPos(int posA, int posB) => (_scrambleText[posB], _scrambleText[posA]) = (_scrambleText[posA], _scrambleText[posB]);

        private void SwapLet(int posA, int posB) => SwapPos(_scrambleText.IndexOf(posA), _scrambleText.IndexOf(posB));

        private void Move(int indexFrom, int indexTo)
        {
            int temp = _scrambleText[indexFrom];

            _scrambleText.RemoveAt(indexFrom);
            _scrambleText.Insert(indexTo, temp);
        }

        private void Rev(int indexFrom, int indexTo) => _scrambleText.Reverse(indexFrom, (indexTo - indexFrom) + 1);

        private void RotLR(int numPos)
        {
            int offset = Math.Abs(numPos) % _scrambleText.Count;
            if (numPos > 0)
            {
                //Rotate Left by an amount...
                _scrambleText = [.. _scrambleText[offset..], .. _scrambleText[..offset]];
            }
            else
            {
                //Rotate Right by an amount...
                _scrambleText = [.. _scrambleText[^offset..], .. _scrambleText[..^offset]];
            }
        }

        private void RotPos(int posA, bool doReversed = false)
        {
            int index = _scrambleText.IndexOf(posA);
            int totalRot = doReversed switch
            {
                //even NB: zero special case, 'cause it just would not work in a formula.
                true => (index == 0)
                                ? -7
                                : (int.IsOddInteger(index))
                                    ? ((index + 1) / 2)      //odd 
                                    : ((index + 4) / 2) - 5, //even
                false => -(1 + index + (index >= 4 ? 1 : 0))
            };

            RotLR(totalRot);
        }

        public void ScrambleText(string toScramble)
        {
            _scrambleText = toScramble.Select(x => (int)x).ToList();

            foreach ((Ops inst, int argA, int argB) in _instructionSet)
            {
                switch (inst)
                {
                    case Ops.Move:
                        Move(argA, argB);
                        break;
                    case Ops.Rev:
                        Rev(argA, argB);
                        break;
                    case Ops.SwapLet:
                        SwapLet(argA, argB);
                        break;
                    case Ops.SwapPos:
                        SwapPos(argA, argB);
                        break;
                    case Ops.RotR:
                        RotLR(-argA);
                        break;
                    case Ops.RotL:
                        RotLR(argA);
                        break;
                    case Ops.RotPos:
                        RotPos(argA);
                        break;
                    case Ops.RotPosRev:
                        RotPos(argA, true);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static (Ops inst, int argA, int argB) ParseInstruction(string instruction, bool doReversed = false)
        {
            Ops inst;
            int argA;
            int argB = -1;

            string[] s = instruction.Split(' ');

            switch (s[0])
            {
                case "move":
                    //move position 7 to position 6
                    inst = Ops.Move;
                    argA = int.Parse(s[2]);
                    argB = int.Parse(s[5]);
                    if (doReversed) (argB, argA) = (argA, argB);

                    break;
                case "reverse":
                    //reverse positions 4 through 7
                    inst = Ops.Rev;
                    argA = int.Parse(s[2]);
                    argB = int.Parse(s[4]);
                    break;
                case "rotate":
                    //rotate [left | right] 1 step
                    //rotate based on position of letter a
                    if (s[1] == "based")
                    {
                        inst = doReversed ? Ops.RotPosRev : Ops.RotPos;
                        argA = s[6][0];
                        //reversal has to happen in RotPos
                    }
                    else
                    {
                        inst = s[1] == "left" ? Ops.RotL : Ops.RotR;
                        if (doReversed) inst = s[1] == "left" ? Ops.RotR : Ops.RotL;
                        argA = int.Parse(s[2]);
                    }
                    break;
                case "swap":
                    //swap letter a with letter c
                    //swap position 0 with position 7
                    if (s[1] == "letter")
                    {
                        inst = Ops.SwapLet;
                        argA = s[2][0];
                        argB = s[5][0];
                    }
                    else
                    {
                        inst = Ops.SwapPos;
                        argA = int.Parse(s[2]);
                        argB = int.Parse(s[5]);
                    }
                    break;
                default:
                    throw new NotImplementedException(s[0]);
            }
            return (inst, argA, argB);
        }
    }
}
