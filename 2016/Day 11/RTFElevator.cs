using System.Text;

namespace AoC_2016_Day_11
{
    internal static class RTFElevator
    {
        public const int INDEX_ELEVATOR = 1;

        private const int FLOOR_BOTTOM = 1;
        private const int FLOOR_TOP = 4;
        private const int INDEX_STEPS = 0;
        private const int START_ITEMS = 2;

        private static readonly Dictionary<string, int> testStates = [];
        private static readonly PriorityQueue<int[], int> queue = new();

        public static int CountTheSteps(int[] initial_state)
        {
            int[] solution = new int[initial_state.Length];
            solution[INDEX_STEPS] = int.MaxValue;

            EnqueueNextStep(initial_state);
            while (queue.TryDequeue(out int[]? current, out int _))
            { 
                if (IsEndState(current))
                {
                    if (current[INDEX_STEPS] < solution[INDEX_STEPS])
                        Array.Copy(current, solution, current.Length);
                    continue;
                }

                // There's nothing left in the queue with a better answer than we have. Also sanity check.
                if (current[INDEX_STEPS] > solution[INDEX_STEPS] || current[INDEX_STEPS] >= 100) continue;

                for (int nextFloor = -1; nextFloor <= 1; nextFloor += 2)
                {
                    if (current[INDEX_ELEVATOR] + nextFloor < FLOOR_BOTTOM || current[INDEX_ELEVATOR] + nextFloor > FLOOR_TOP) continue;

                    for (int itemOne = START_ITEMS; itemOne < current.Length; itemOne++)
                    {
                        if (current[itemOne] == current[INDEX_ELEVATOR])
                        {
                            int[] nextStepOne = new int[current.Length];
                            Array.Copy(current, nextStepOne, current.Length);

                            nextStepOne[INDEX_STEPS]++;
                            nextStepOne[INDEX_ELEVATOR] += nextFloor;
                            nextStepOne[itemOne] += nextFloor;

                            // If valid, queue it. Don't stop though, because it and something else might be valid.
                            if (IsValidState(nextStepOne)) EnqueueNextStep(nextStepOne);

                            for (int itemTwo = START_ITEMS; itemTwo < current.Length; itemTwo++)
                            {
                                if (nextStepOne[itemTwo] == current[INDEX_ELEVATOR])
                                {
                                    int[] nextStepTwo = new int[current.Length];
                                    Array.Copy(nextStepOne, nextStepTwo, nextStepOne.Length);

                                    nextStepTwo[itemTwo] += nextFloor;
                                    if (IsValidState(nextStepTwo))
                                        EnqueueNextStep(nextStepTwo);
                                }
                            }
                        }
                    }
                }
            }

            return solution[INDEX_STEPS];
        }

        private static bool IsEndState(int[] state) => state.Skip(START_ITEMS).All(x => x == FLOOR_TOP);

        private static string HashState(int[] state)
        {
            StringBuilder sb = new();
            sb.Append(state[INDEX_ELEVATOR]);
            for (int f = FLOOR_BOTTOM;  f <= FLOOR_TOP; f++)
            {
                int numPairs = 0;
                int numChips = 0;
                int numGens = 0;

                for (int itm = START_ITEMS; itm < state.Length; itm += 2)
                {
                    if (state[itm] == f && state[itm + 1] == f) numPairs++;
                    if (state[itm] == f && state[itm + 1] != f) numGens++;
                    if (state[itm] != f && state[itm + 1] == f) numChips++;
                }
                sb.Append($"{f}{numPairs}{numChips}{numGens}");
            }
            return sb.ToString();
        }

        private static bool IsValidState(int[] state)
        {
            //are there any unshielded microchips on the same floor as any generators? 
            for (int floor = FLOOR_BOTTOM; floor <= FLOOR_TOP; floor++)
            {
                bool isGen = false;
                bool isVulnChip = false;

                for (int itm = START_ITEMS; itm < state.Length; itm += 2)
                {
                    if (state[itm] == floor) isGen = true;
                    if (state[itm + 1] == floor && state[itm] != floor) isVulnChip = true;
                    if (isGen && isVulnChip) return false;
                }
            }

            return true;
        }

        private static void EnqueueNextStep(int[] nextStep)
        {
            if (testStates.TryGetValue(HashState(nextStep), out int numSteps))
            {
                if (nextStep[INDEX_STEPS] < numSteps)
                {
                    testStates[HashState(nextStep)] = nextStep[INDEX_STEPS];
                    queue.Enqueue((int[])nextStep.Clone(), nextStep[INDEX_STEPS]);
                }
            }
            else
            {
                testStates.Add(HashState(nextStep), nextStep[INDEX_STEPS]);
                queue.Enqueue((int[])nextStep.Clone(), nextStep[INDEX_STEPS]);
            }
        }
    }
}
