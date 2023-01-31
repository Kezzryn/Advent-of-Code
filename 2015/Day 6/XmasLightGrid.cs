using System.Drawing;
using System.Text.RegularExpressions;

namespace AoC_2015_Day_6
{
    internal static class RuleSet
    {
        public const int Toggle = 0;
        public const int Brighten = 1;
    }

    internal partial class XmasLightGrid
    {
        private readonly Dictionary<Point, int> _lightGrid = new();
        private readonly int ToggleOrBright;

        public XmasLightGrid(int ruleSet = RuleSet.Toggle)
        {
            ToggleOrBright = ruleSet;
        }

        private void FlipLights(int x1, int y1, int x2, int y2, char action)
        {
            // data is always bottom left to upper right.

            Point cursor = new (0,0);

            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    cursor.X = x;
                    cursor.Y = y; 
                    if (_lightGrid.TryGetValue(cursor, out int light))
                    {
                        _lightGrid[cursor] += action switch
                        {
                            'T' => (ToggleOrBright == RuleSet.Toggle) ? (light == 1) ? -1 : 1 : 2,
                            'O' => (ToggleOrBright == RuleSet.Toggle) ? (light == 1) ? 0 : 1 : 1,
                            'F' => (ToggleOrBright == RuleSet.Toggle) ? (light == 1) ? -1 : 0 : -1,
                            _ => 0
                        };
                        if (_lightGrid[cursor] < 0) _lightGrid[cursor] = 0;
                    }
                    else
                    {
                        if (ToggleOrBright == RuleSet.Toggle)
                        {
                            _lightGrid.Add(cursor, (action != 'F') ? 1 : 0) ;
                        }
                        else
                        {
                            _lightGrid.Add(cursor, action switch { 'T' => 2, 'O' => 1, _ => 0 });
                        }
                        
                    }
                }
            }
        }

        public void Instruction(string instruction)
        {
            int[] numbers = FindNumbers().Matches(instruction).Select(x => int.Parse(x.Value)).ToArray();

            if (instruction.StartsWith("toggle")) Toggle(numbers[0], numbers[1], numbers[2], numbers[3]);

            if (instruction.StartsWith("turn on")) TurnOn(numbers[0], numbers[1], numbers[2], numbers[3]);

            if (instruction.StartsWith("turn off")) TurnOff(numbers[0], numbers[1], numbers[2], numbers[3]);
        }

        public void Toggle(int x1, int y1, int x2, int y2)
        {
            FlipLights(x1, y1, x2, y2, 'T');
        }

        public void TurnOn(int x1, int y1, int x2, int y2)
        {
            FlipLights(x1, y1, x2, y2, 'O');
        }

        public void TurnOff(int x1, int y1, int x2, int y2)
        {
            FlipLights(x1, y1, x2, y2, 'F');
        }
        public int NumLit() => _lightGrid.Where(pair => pair.Value >= 0).Count();

        public int Luminosity() => _lightGrid.Select(pair => pair.Value).Sum();

    }
    partial class XmasLightGrid
    {
        [GeneratedRegex("\\d+")]
        private static partial Regex FindNumbers();
    }
 
}
