using System.Numerics;

namespace AoC_2022_Day_13
{
    internal static class ReturnValues
    {
        //human readable return types
        public const int LessThan = -1;
        public const int EqualTo = 0;
        public const int GreaterThan = 1;
        public const int Unknown = 2;
    }

    internal class Packet : IComparable<Packet>, IEquatable<Packet>, IComparisonOperators<Packet, Packet, bool>
    {
        private string _packetValue = "";
        public string Value
        {
            get
            {
                return _packetValue;
            }
            set
            {
                _packetValue = value;
                ElementList = PacketToList(value);
            }
        }
        public List<string> ElementList { get; set; }

        public Packet(string inputPacket)
        {
            ElementList = new();
            _packetValue = inputPacket;
            ElementList = PacketToList(inputPacket);
        }
        public Packet()
        {
            ElementList = new();
            _packetValue = "";
        }

        private static int FindMatchingBrace(string inputPacket, int startElement)
        {
            int returnValue = -1;
            int matchCount = 0;

            for (int i = startElement; i < inputPacket.Length; i++)
            {
                if (inputPacket[i] == '[') { matchCount++; }
                if (inputPacket[i] == ']') { matchCount--; }
                if (matchCount == 0)
                {
                    returnValue = i;
                    break;
                }
            }
            return returnValue;
        }

        private static List<string> PacketToList(string inputPacket)
        {
            //Convert the packet to an List<string>.
            //Each element of the List will contain one element of the inputPacket, which could be an integer, a list, or even an empty list
            List<string> returnValue = new();

            if (inputPacket == "[]")
            {
                returnValue.Add(string.Empty);
                return returnValue;
            }

            //Do we have any brackets?
            if (inputPacket.Contains('[') == false || inputPacket.Contains(']') == false)
            {
                //no brackets at all. We might be a list. Do we have commas? 
                if (inputPacket.Contains(','))
                    returnValue.AddRange(inputPacket.Split(','));
                else
                    returnValue.Add(inputPacket);

                return returnValue;
            }

            //if we got this far, we have some brackets.
            //so now the heavy lifting starts. We're going to take the current packet and turn it into a list, which might contain nested lists.  
            //[],[1,[2,3,4],5],6] will return a list with three elements: {""}, {1,[2,3,4],5}, {6}  
            int startElement = 0;
            int endElement = 0;
            while (startElement < inputPacket.Length)
            {
                if (inputPacket[startElement] == '[')
                {
                    endElement = FindMatchingBrace(inputPacket, startElement);

                    if (endElement == -1)
                    {
                        returnValue.Add(string.Empty);
                        break;
                    }
                    startElement++; // shunt over to kill opening bracket 
                }
                else
                {
                    endElement = inputPacket.IndexOf(",", startElement);
                    if (endElement == -1) endElement = inputPacket.Length;
                }

                returnValue.Add(inputPacket[startElement..endElement]);

                startElement = inputPacket.IndexOf(',', endElement);
                startElement = (startElement == -1) ? inputPacket.Length : startElement += 1;
            }

            return returnValue;
        }

        private static int Compare(Packet? leftPacket, Packet? rightPacket)
        {
            //null tests. 
            if (leftPacket is null && rightPacket is not null) return ReturnValues.LessThan;
            if (leftPacket is not null && rightPacket is null) return ReturnValues.GreaterThan;
            if (leftPacket is null && rightPacket is null) return ReturnValues.EqualTo;

            //let's get rid of all the compiler warnings about nulls...  NB: this should never trigger 
            if (leftPacket is null || rightPacket is null) return ReturnValues.EqualTo;

            //Unknown is a self-flag that we need to keep processing. It should never be passed back to a comparison function.
            int returnValue = ReturnValues.Unknown;

            //rules. 
            //if both are ints return if leftInt < rightInt 
            //if right list is longer, then

            for (int i = 0; i < leftPacket.ElementList.Count; i++)
            {
                //If the right list runs out of items first, then left is GT right
                if (i > rightPacket.ElementList.Count - 1)
                {
                    returnValue = ReturnValues.GreaterThan;
                    break;
                }

                string leftElement = leftPacket.ElementList.ElementAt(i);
                string rightElement = rightPacket.ElementList.ElementAt(i);

                //check integer comparisons 
                if (int.TryParse(leftElement, out int leftInt) && int.TryParse(rightElement, out int rightInt))
                {
                    if (leftInt < rightInt) returnValue = ReturnValues.LessThan;
                    if (leftInt > rightInt) returnValue = ReturnValues.GreaterThan;
                    if (leftInt == rightInt) returnValue = ReturnValues.Unknown; //this might not be the end of the comparisons... 
                }
                else
                {
                    if (leftElement == string.Empty && rightElement == string.Empty)
                        returnValue = ReturnValues.Unknown;  //bothsides ran out together, but we might have more to check.  
                    else if (leftElement == string.Empty && rightElement != string.Empty)
                        returnValue = ReturnValues.LessThan; //leftside ran out first 
                    else if (leftElement != string.Empty && rightElement == string.Empty)
                        returnValue = ReturnValues.GreaterThan; //right side ran out first 
                    else
                        //not integers, and not empty, so we have lists, so recurse this function.
                        returnValue = Compare(new Packet(leftElement), new Packet(rightElement));
                }


                if (i == (leftPacket.ElementList.Count - 1) && returnValue == ReturnValues.Unknown)
                {
                    //If the left list runs out of items first, the inputs are in the right order.
                    if (rightPacket.ElementList.Count - 1 > i) returnValue = ReturnValues.LessThan;
                    //Last resort, return EqualTo, 'cause we're at the end of the left lift and have exhausted all other possibilities. 
                    if (rightPacket.ElementList.Count == leftPacket.ElementList.Count) returnValue = ReturnValues.EqualTo;
                }

                if (returnValue == ReturnValues.LessThan || returnValue == ReturnValues.GreaterThan) break; // early break for known good return values.
            }

            return returnValue;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Packet)
                return CompareTo(obj);
            else
                throw new ArgumentException("Object is not a Packet");
        }

        public int CompareTo(Packet? other) => Compare(this, other);

        public bool Equals(Packet? other) => Compare(this, other) == ReturnValues.EqualTo;

        public static bool operator >(Packet left, Packet right) => Compare(left, right) == ReturnValues.GreaterThan;

        public static bool operator >=(Packet left, Packet right)
        {
            int rv = Compare(left, right);
            return (rv == ReturnValues.GreaterThan || rv == ReturnValues.EqualTo);
        }

        public static bool operator <(Packet left, Packet right) => Compare(left, right) == ReturnValues.LessThan;

        public static bool operator <=(Packet left, Packet right)
        {
            int rv = Compare(left, right);
            return (rv == ReturnValues.LessThan || rv == ReturnValues.EqualTo);
        }

        public static bool operator ==(Packet? left, Packet? right) => Compare(left, right) == ReturnValues.EqualTo;

        public static bool operator !=(Packet? left, Packet? right) => Compare(left, right) != ReturnValues.EqualTo;

        public override bool Equals(object? obj) => Equals(obj as Packet);

        public override int GetHashCode() => Value.GetHashCode();
    }
}

