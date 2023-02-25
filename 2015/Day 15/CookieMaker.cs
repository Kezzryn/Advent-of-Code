namespace AoC_2015_Day_15
{
    enum CookieProperties
    {
        Capacity,
        Durability,
        Flavor,
        Texture,
        Calories
    };

    internal class CookieMaker
    {
        private readonly int _maxCalories; // -1 is no max

        // I should generate these to fit the size of the ingredient array.
        private readonly List<int[]> _stepKey = new()
        {
            new int[] { +1, -1,  0,  0 },
            new int[] { +1,  0, -1,  0 },
            new int[] { +1,  0,  0, -1 },
            new int[] { +2, -1, -1,  0 },
            new int[] { +2, -1,  0, -1 },
            new int[] { +2,  0, -1, -1 },
            new int[] { +3, -1, -1, -1 },
        };

        // I should also generate these to fit the size of the ingredient array.
        private readonly List<int[]> _startKeys = new()
        {
            new int[] { 25, 25, 25, 25 },
            new int[] { 73,  9,  9,  9 },
            new int[] {  9, 73,  9,  9 },
            new int[] {  9,  9, 73,  9 },
            new int[] {  9,  9,  9, 73 },
        };

        private readonly Dictionary<int, Dictionary<CookieProperties, int>> _ingredients = new();

        public CookieMaker(string[] puzzleInput, int maxCalories = -1)
        {
            _maxCalories = maxCalories;

            for (int i = 0; i < puzzleInput.Length; i++)
            {
                _ingredients.Add(i, new());

                foreach (CookieProperties property in Enum.GetValues(typeof(CookieProperties)))
                {
                    // Find the space after the lower case-d name of our current property
                    int startIndex = puzzleInput[i].IndexOf(' ', puzzleInput[i].IndexOf(Enum.GetName(typeof(CookieProperties), property)!.ToLower()));

                    // find the next comma, then test to see if we're off the end of the string. 
                    int length = puzzleInput[i].IndexOf(',', startIndex);
                    length = (length == -1) ? puzzleInput[i].Length - startIndex : length - startIndex;

                    _ingredients[i].Add(property, int.Parse(puzzleInput[i].Substring(startIndex, length)));
                }
            }
        }

        // Our maximization function.
        private int ValueFunction(int[] recipe)
        {
            int returnValue = 1;
            foreach (CookieProperties property in Enum.GetValues(typeof(CookieProperties)))
            {
                int subValue = 0;
                for (int i = recipe.GetLowerBound(0); i <= recipe.GetUpperBound(0); i++)
                {
                    subValue += recipe[i] * _ingredients[i][property];
                }

                if (property != CookieProperties.Calories)
                {
                    if (subValue <= 0) return 0; // no negatives permitted, and 0s will zero out the entire calculation. 
                    returnValue *= subValue;
                }
                else
                {
                    if (_maxCalories < 0) continue;  // Skip the following if we're not counting calories.
                    if (subValue == _maxCalories) continue; // Continue if we're on target
                    return 0; // This one's no good to us.
                }
            }
            return returnValue;
        }

        // This is a hack to covert an array to a string in a way we can generate short unique keys out of it. 
        private static string GetAnswerKey(int[] potentialAnswer) => potentialAnswer.Select(x => x.ToString()).Aggregate((x, y) => x + y);

        public int GetBestCookie()
        {
            int loopcounter = 0;

            // HashSet is because I can't view the queue for duplicate entries.
            PriorityQueue<int[], int> queue = new();
            HashSet<string> queuedSolutions = new();
            
            int bestAnswer = int.MinValue;

            // Prepopulate the search queue with some potential answers.
            foreach (int[] keys in _startKeys)
            {
                queue.Enqueue(keys, -ValueFunction(keys));
                queuedSolutions.Add(GetAnswerKey(keys));
            }

            while (queue.Count > 0)
            {
                loopcounter++;
                
                if (!queue.TryDequeue(out int[]? potentialAnswer, out int priority)) break;
                
                // Pull the priority off the queue, rather than re-compute it. 
                priority *= -1; // ...and invert it, 'cause we push it to the queue as a negative. 

                // Do we have a better answer?
                if (priority > bestAnswer) bestAnswer = priority;

                foreach (int[] step in _stepKey)
                {
                    // the i, j loop is to iterate / move the start point of the step keys.
                    // we'll access the indexes 0123, then 1230, 2301, etc.
                    for (int i = 0; i < step.Length; i++)
                    {
                        int[] newAnswer = new int[step.Length];
                        for (int j = 0; j < step.Length; j++)
                        {
                            int index = (step.Length + i + j) % step.Length;

                            // basic bounds check. This could probably be improved.
                            newAnswer[j] = int.Max(0, int.Min(step[index] + potentialAnswer[index], 100));
                        }
                        if (newAnswer.Sum() != 100) continue; // Sanity / bounds check.

                        if (!queuedSolutions.Contains(GetAnswerKey(newAnswer)))
                        {
                            int newValue = ValueFunction(newAnswer);
                            // Only Enqueue equal or 'better' solutions.
                            // If our neighbors are equal, we might be on a shoulder / flat.
                            if (newValue >= priority) 
                            {
                                queue.Enqueue(newAnswer, -newValue);
                                queuedSolutions.Add(GetAnswerKey(newAnswer));
                            }
                        }
                    }
                }
            }
            return bestAnswer;
        }
    }
}
