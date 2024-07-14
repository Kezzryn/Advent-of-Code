namespace AoC_2018_Day_03;
using BKH.Geometry;
internal class RectGroup
{
    public readonly List<Rectangle> Rectangles = [];
    private readonly Queue<Rectangle> queue = [];

    public void Add(Rectangle newRect)
    {
        queue.Enqueue(newRect);

        while (queue.TryDequeue(out Rectangle? queueRect))
        {
            int stopIndex = Rectangles.Count - 1;
            for (int i = 0; i <= stopIndex; i++)
            {
                if (queueRect.Overlap(Rectangles[i]))
                {
                    Rectangles.AddRange(Rectangles[i].Exclude(queueRect));
                    Rectangles.RemoveAt(i--);
                    stopIndex--;
                }
            }

            Rectangles.Add(queueRect);
        }
    }
}