using BKH.Geometry.Rectangle;
using System.Text.RegularExpressions;
using static AoC_2015_Day_06.XmasLightGrid;

namespace AoC_2015_Day_06
{
    internal partial class XmasLightGrid(Ruleset ruleSet)
    {
        private readonly List<Rectangle> _lights = [];
        private readonly List<int> _lightBrightness = [];
        private readonly Ruleset _ruleSet = ruleSet;
        public string LightCount => $"{_lights.Count}";

        private void BrightenLights(Rectangle flipReq, Action action)
        {
            IEnumerable<(Rectangle rect, int i)> overlapList = _lights.Select((x, i) => (x,i)).Where(x => flipReq.Overlap(x.x));

            List<(Rectangle rect, int brightness)> intersections = [];
            List<(Rectangle rect, int brightness)> existingRects = [];

            foreach((Rectangle rectangle, int i) in overlapList)
            {
                int currentBrightness = _lightBrightness[i];
                int newBrightness = currentBrightness + action switch
                {
                    Action.Toggle =>   2,
                    Action.TurnOff => -1,
                    Action.TurnOn =>   1,
                    _ => throw new NotImplementedException($"action {action}")
                };

                intersections.Add((rectangle.Intersect(flipReq), newBrightness));
                existingRects.AddRange(rectangle.Exclude(flipReq).Select(x => (x,currentBrightness)));
            }

            foreach(int i in overlapList.OrderByDescending(x => x.i).Select(x => x.i))
            {
                _lights.RemoveAt(i);
                _lightBrightness.RemoveAt(i);
            }

            _lights.AddRange(intersections.Where(x => x.brightness > 0).Select(x => x.rect));
            _lightBrightness.AddRange(intersections.Where(x => x.brightness > 0).Select(x => x.brightness));

            _lights.AddRange(existingRects.Select(x => x.rect));
            _lightBrightness.AddRange(existingRects.Select(x => x.brightness));

            //carved down chunk. 
            if (action == Action.TurnOn || action == Action.Toggle)
            {
                Queue<Rectangle> queue = new();
                queue.Enqueue(flipReq);
                List<Rectangle> newRects = [];
                while (queue.TryDequeue(out Rectangle? rect))
                {
                    bool isCutDown = false;
                    foreach(Rectangle interRect in intersections.Select(x => x.rect))
                    {
                        if (rect.Overlap(interRect))
                        {
                            isCutDown = true;
                            rect.Exclude(interRect).ForEach(queue.Enqueue);
                            break;
                        }
                    }

                    if(!isCutDown) newRects.Add(rect);
                }

                foreach (Rectangle rect in newRects)
                {
                    _lights.Add(rect);
                    _lightBrightness.Add(action == Action.TurnOn ? 1 : 2);
                }
            }
        }

        private void ToggleLights(Rectangle flipReq, Action action)
        {
            Queue<Rectangle> queue = [];
            queue.Enqueue(flipReq);

            while (queue.TryDequeue(out Rectangle? newReq))
            {
                bool isOverlap = false;
                for (int i = 0; i < _lights.Count; i++)
                {
                    if (newReq.Overlap(_lights[i]))
                    {
                        isOverlap = true;
                        if (action == Action.Toggle)
                        {
                            newReq.Exclude(_lights[i]).ForEach(queue.Enqueue);

                            _lights.AddRange(_lights[i].Exclude(newReq));
                            _lights.RemoveAt(i--);
                            break; //We need to stop using the current shape. We might have more overlaps with the other bits we just queued.
                        }

                        _lights.AddRange(_lights[i].Exclude(newReq));
                        _lights.RemoveAt(i--);
                    }
                }

                if (action == Action.TurnOn || (action == Action.Toggle && !isOverlap))
                {
                    _lights.Add(newReq);
                }
            }
        }

        public void Instruction(string instruction)
        {
            int[] numbers = FindNumbers().Matches(instruction).Select(x => int.Parse(x.Value)).ToArray();
            Rectangle newReq = new(numbers[0], numbers[1], numbers[2], numbers[3]);
            Action action;

            if (instruction.StartsWith("toggle"))
                action = Action.Toggle;
            else if (instruction.StartsWith("turn on"))
                action = Action.TurnOn;
            else if (instruction.StartsWith("turn off"))
                action = Action.TurnOff;
            else
                throw new Exception($"Unknown instruction. {instruction}");

            if (_ruleSet == Ruleset.Toggle) ToggleLights(newReq, action);
            if (_ruleSet == Ruleset.Brighten) BrightenLights(newReq, action);
        }

        public long NumLit() => _lights.Sum(x => x.Area);

        public long Luminosity() => _lightBrightness.Select((x, i) => _lights[i].Area * x).Sum();

        private enum Action
        {
            TurnOn,
            TurnOff,
            Toggle
        }

        public enum Ruleset
        {
            Toggle,
            Brighten
        }
    }
    partial class XmasLightGrid
    {
        [GeneratedRegex("\\d+")]
        private static partial Regex FindNumbers();
    }
}
