using System.Text;

namespace AoC_2016_Day_21
{
    enum Ops
    {
        Move,
        Rev,
        RotL,
        RotPos,
        RotR,
        SwapPos,
        SwapLet
    };

    internal class Scrambler
    {
        private string _scrambleText;
        private readonly bool _doReversed;

        public Scrambler(string text, bool doReversed = false)
        {
            _scrambleText = text;
            _doReversed = doReversed;
        }

        public override string ToString() => _scrambleText;

        private void SwapPos(int posA, int posB)
        {
            char[] temp = _scrambleText.ToCharArray();
            (temp[posB], temp[posA]) = (temp[posA], temp[posB]); // Yay intellisense
            _scrambleText = new(temp);
        }

        private void SwapLet(int posA, int posB)
        {
            SwapPos(_scrambleText.IndexOf((char)posA), _scrambleText.IndexOf((char)posB));
        }

        private void Move(int indexFrom, int indexTo)
        {
            StringBuilder sb = new(_scrambleText);
            sb.Remove(indexFrom, 1);
            sb.Insert(indexTo, _scrambleText[indexFrom]);
            _scrambleText = new(sb.ToString());

        }

        private void Rev(int indexFrom, int indexTo)
        {
            char[] temp = _scrambleText.ToCharArray();
            Array.Reverse(temp, indexFrom, (indexTo - indexFrom) + 1);
            _scrambleText = new(temp);
        }

        private void RotLR(int numPos)
        {
            int offset = Math.Abs(numPos) % _scrambleText.Length;

            string temp;
            if (numPos > 0)
            {
                //Rotate Left by an amount...
                temp = _scrambleText[offset..] + _scrambleText[..offset];
            }
            else
            {
                //Rotate Right by an amount...
                temp = _scrambleText[^offset..] + _scrambleText[..^offset];
            }

            _scrambleText = temp;
        }

        private void RotPos(int posA)
        {
            int index = _scrambleText.IndexOf((char)posA);
            int totalRot;
            if (_doReversed)
            {
                //even NB: zero special case, 'cause it just would not work in a formula.
                totalRot = (index == 0)
                                ? -7
                                : ((index % 2) == 1)
                                    ? ((index + 1) / 2)      //odd 
                                    : ((index + 4) / 2) - 5; //even
            }
            else
            {
                totalRot = -(1 + index + (index >= 4 ? 1 : 0));
            }

            RotLR(totalRot);
        }

        public void Step(string instruction)
        {
            ParseInstruction(instruction, out Ops inst, out int argA, out int argB);
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
                    RotPos((char)argA);
                    break;
                default:
                    throw new NotImplementedException(instruction);
            }
        }

        private void ParseInstruction(string instruction, out Ops inst, out int argA, out int argB)
        {
            inst = 0;
            argA = -1;
            argB = -1;

            string[] s = instruction.Split(' ');

            switch (s[0])
            {
                case "move":
                    //move position 7 to position 6
                    inst = Ops.Move;
                    argA = int.Parse(s[2]);
                    argB = int.Parse(s[5]);
                    if (_doReversed) (argB, argA) = (argA, argB);

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
                        inst = Ops.RotPos;
                        argA = s[6][0];
                        //reversal has to happen in RotPos
                    }
                    else
                    {
                        inst = s[1] == "left" ? Ops.RotL : Ops.RotR;
                        if (_doReversed) inst = s[1] == "left" ? Ops.RotR : Ops.RotL;
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
        }

    }
}
