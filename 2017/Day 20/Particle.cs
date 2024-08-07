﻿namespace AoC_2017_Day_20;
using BKH.Geometry;

internal class Particle
{
    public int ID { get; }

    public Point3D Pos { get; set; } = new();

    public int Dist => Point3D.TaxiDistance3D(Pos);

    private Point3D _velocity = new();

    private readonly Point3D _accel = new();

    public Particle(string data, int id)
    {
        ID = id;

        foreach (string coord in data.Split(", "))
        {
            int[] xyz = coord[3..^1].Split(',').Select(int.Parse).ToArray();

            if (coord[0] == 'p') Pos = new Point3D(xyz);
            if (coord[0] == 'v') _velocity = new Point3D(xyz);
            if (coord[0] == 'a') _accel = new Point3D(xyz);
        }
    }

    public void Step()
    {
        _velocity += _accel;
        Pos += _velocity;
    }
}
