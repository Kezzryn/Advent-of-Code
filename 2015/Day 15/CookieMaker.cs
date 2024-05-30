namespace AoC_2015_Day_15
{
    internal class CookieMaker
    {
        private int _targetCalories; // -1 is no target

        // I should generate these to fit the size of the ingredient array.
        private static readonly List<int[]> _stepKey =
        [
            [+1, -1,  0,  0],
            [+1,  0, -1,  0],
            [+1,  0,  0, -1],
            [+2, -1, -1,  0],
            [+2, -1,  0, -1],
            [+2,  0, -1, -1],
            [+3, -1, -1, -1],
        ];

        private static readonly Dictionary<string, int> _cookieProperties = new()
        {
            { "capacity", 1 },
            { "durability", 2 },
            { "flavor", 3 },
            { "texture", 4 },
            { "calories", 5}
        };

        private readonly List<Dictionary<int, int>> _ingredients = [];

        public CookieMaker(string[] puzzleInput, int targetCalories = -1)
        {
            _targetCalories = targetCalories;

            foreach(string line in puzzleInput)
            {
                Dictionary<int, int> newIngredient = [];

                foreach ((string property, int propIndex) in _cookieProperties)
                {
                    // Find the space after the lower case-d name of our current property
                    int startIndex = line.IndexOf(' ', line.IndexOf(property));

                    // find the next comma, then test to see if we're off the end of the string. 
                    int length = line.IndexOf(',', startIndex);
                    length = (length == -1) ? line.Length - startIndex : length - startIndex;

                    newIngredient.Add(propIndex, int.Parse(line.Substring(startIndex, length)));
                }
                _ingredients.Add(newIngredient);
            }
        }

        public int TargetCalories { get { return _targetCalories; } set {  _targetCalories = value; } }

        // Our maximization function.
        private int ValueFunction(int[] recipe)
        {
            int returnValue = 1;
            foreach ((string property, int propIndex) in _cookieProperties)
            {
                int subValue = 0;
                for (int i = recipe.GetLowerBound(0); i <= recipe.GetUpperBound(0); i++)
                {
                    subValue += recipe[i] * _ingredients[i][propIndex];
                }

                if (property != "calories")
                {
                    if (subValue <= 0) return 0; // no negatives permitted, and 0s will zero out the entire calculation. 
                    returnValue *= subValue;
                }
                else
                {
                    // Part 2 check for target calories. 
                    if (_targetCalories > 0 && subValue != _targetCalories) return 0;
                }
            }
            return returnValue;
        }

        // This is a hack to covert an array to a string in a way we can generate short unique keys out of it. 
        private static string GetAnswerKey(int[] potentialAnswer) => potentialAnswer.Select(x => x.ToString()).Aggregate((x, y) => x + y);

        public int GetBestCookie()
        {
            // HashSet is because I can't view the queue for duplicate entries.
            PriorityQueue<int[], int> queue = new();
            HashSet<string> queuedSolutions = [];

            int[] startKey = [25, 25, 25, 25];
            int bestAnswer = int.MinValue;

            // Prepopulate the search queue with a potential answer.
            // Adding a variety of start keys does not seem to speed up the search
            queue.Enqueue(startKey, -ValueFunction(startKey));
            queuedSolutions.Add(GetAnswerKey(startKey));

            int[] newAnswer = new int[_stepKey[0].Length];
            int keyIndex;
            int newValue;
            while (queue.TryDequeue(out int[]? potentialAnswer, out int currentAnswer))
            {
                // Pull the priority off the queue, rather than re-compute it.
                // ...and invert it, 'cause we push it to the queue as a negative. 
                currentAnswer = -currentAnswer; 

                // Do we have a better answer?
                if (currentAnswer > bestAnswer) bestAnswer = currentAnswer;

                foreach (int[] step in _stepKey)
                {
                    // the i, j loop is to iterate / move the start point of the step keys.
                    // we'll access the indexes 0123, then 1230, 2301, etc.
                    for (int i = 0; i < step.Length; i++)
                    {
                        Array.Clear(newAnswer);
                        for (int j = 0; j < step.Length; j++)
                        {
                            keyIndex = (step.Length + i + j) % step.Length;

                            // basic bounds check. This could probably be improved.
                            newAnswer[j] = int.Max(0, int.Min(step[keyIndex] + potentialAnswer[keyIndex], 100));
                        }
                        if (newAnswer.Sum() != 100) continue; // Sanity / bounds check.

                        if (!queuedSolutions.Contains(GetAnswerKey(newAnswer)))
                        {
                            newValue = ValueFunction(newAnswer);
                            // Only Enqueue equal or 'better' solutions.
                            // If our neighbors are equal, we might be on a shoulder / flat.
                            if (newValue >= currentAnswer) 
                            {
                                queue.Enqueue([..newAnswer], -newValue);        //copy the newAnswer array, or have ref vs. val issues.
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
