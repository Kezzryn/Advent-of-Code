﻿namespace AoC_2022_Day_22
{
    internal class Map
    {
        // This represents one side of a cube.
        // We store links to other maps, and the rotation between this map and the next as a degree. (90, 180, 270, or negative) 

        private const char WALL = '#';

        private readonly string _theMap;
        private readonly int _interval;

        private int[] _links = new int[4];
        private int[] _rotation = new int[4];

        public int GridX { get; }
        public int GridY { get; }
        public int ID { get; }
        public Map(string mapData, int interval, int gridX, int gridY, int id)
        {
            _theMap = mapData;
            _interval = interval;
            GridX = gridX;
            GridY = gridY;
            ID = id;
        }

        public bool IsOnMap(Cursor cursor) => cursor.X >= 1 && cursor.Y >= 1 && cursor.X <= _interval && cursor.Y <= _interval;
        public bool IsWall(Cursor cursor) => MapAt(cursor.X, cursor.Y) == WALL;
        public bool IsWall(int x, int y) => MapAt(x,y) == WALL;
        private char MapAt(int x, int y) => _theMap[x-1 + ((y-1)*_interval)];
        public Tuple<int, int> TranslateToSource(Cursor cursor) => new((GridX * _interval) + cursor.X, (GridY * _interval) + cursor.Y);
        public void SetLink(int direction, int link, int rotation = 0)
        {
            _links[direction] = link;
            _rotation[direction] = rotation;
        }

        public void SetLink(int[] link, int[] rotation)
        {
            // each array location needs to match the Directions constants or things will get weird.
            for (int i = 0;i < link.Length; i++)
            {
                _links[i] = link[i];
                _rotation[i] = rotation[i];
            }
        }
        public int GetLink(int direction) => _links[direction];
        public int GetRotation(int direction) => _rotation[direction];
        public int GetInterval() => _interval;
    }
}
