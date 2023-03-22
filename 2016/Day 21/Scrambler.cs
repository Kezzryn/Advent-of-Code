using System.ComponentModel.Design;
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
        private bool _doReversed;

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
            int len = _scrambleText.Length;
            //positive rotations are to the left.   
            char[] temp = new char[len];

            for (int i = 0; i < _scrambleText.Length; i++)
            {
                int offset = (i + Math.Abs(numPos)) % len;

                if (numPos > 0)
                {
                    temp[i] = _scrambleText[offset];
                }
                else
                {
                    temp[offset] = _scrambleText[i];
                }
            }

            _scrambleText = new(temp);
        }

        private void RotPos(int posA)
        {
            int index = _scrambleText.IndexOf((char)posA);
            int totalRot = 1 + index + (index >= 4 ? 1 : 0);
            
            RotLR(_doReversed ? totalRot : -totalRot);
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
                        argA = int.Parse(s[2]);
                        if (_doReversed) inst = (inst == Ops.RotL) ? Ops.RotR : Ops.RotL; 
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
                        if (_doReversed) (argB, argA) = (argA, argB);
                    }
                    else
                    {
                        inst = Ops.SwapPos;
                        argA = int.Parse(s[2]);
                        argB = int.Parse(s[5]);
                        if (_doReversed) (argB, argA) = (argA, argB);
                    }
                    break;
                default:
                    throw new NotImplementedException(s[0]);
            }
        }

    }
}
