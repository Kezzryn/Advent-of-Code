namespace AoC_2022_Day_22
{
    internal static class Directions
    {
        public const int Right = 0;
        public const int Down = 1;
        public const int Left = 2;
        public const int Up = 3;
    }

    internal record Cursor
    {
        public int X;
        public int Y;
        public int Facing; //this should match to an entry in the Directions class.
        public int MapIndex;

        public Cursor()
        {
            X = 1;
            Y = 1;
            Facing = Directions.Right;
            MapIndex = 0;
        }

        public Cursor ShallowCopy()
        {
            return (Cursor) MemberwiseClone();
        }

        public void Turn(string direction)
        {
            Facing = direction switch
            {
                "L" => ((Facing - 1) < 0) ? 3 : Facing - 1,
                "R" => ((Facing + 1) > 3) ? 0 : Facing + 1,
                _ => throw new Exception($"Unknown direction {direction}.")
            };
        }

        public void Step()
        {
            X += Facing switch
            {
                Directions.Right => 1,
                Directions.Left => -1,
                _ => 0
            };

            Y += Facing switch
            {
                Directions.Up => -1,
                Directions.Down => 1,
                _ => 0
            };
        }
    }
}
