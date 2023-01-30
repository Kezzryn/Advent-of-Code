using System.Drawing;

namespace AoC_2015_Day_6
{
    internal class XmasLightGrid
    {
        private readonly Dictionary<Point, bool> _lightGrid = new();

        public XmasLightGrid()
        {

        }

        private void FlipLights(int x1, int y1, int x2, int y2, char action)
        {
            int right = int.Max(x1, x2);
            int top = int.Max(y1, y2);

            int left = int.Min(x1, x2);
            int bottom = int.Min(y1, y2);

            for (int x = left; x <= right; x++)
            {
                for (int y = bottom; y <= top; y++)
                {
                    if (_lightGrid.TryGetValue(new Point(x, y), out bool light))
                    {
                        light = action switch
                        {
                            'T' => !light,
                            'O' => true,
                            'F' => false,
                            _ => false
                        };
                    }
                    else
                    {
                        _lightGrid.Add(new Point(x, y), action != 'F');
                    }
                }
            }
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

        public int NumLit() => _lightGrid.Where(pair => pair.Value == true).Count();

    }
}
