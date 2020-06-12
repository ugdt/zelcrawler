using Godot;
using System;
using System.Drawing;

public class PanningCamera : Camera2D
{
    [Signal]
    public delegate void moved(int x, int y, int radius);

    [Export] public int Radius = 15;

    private float speed = 16 * 2;
    private readonly Random random = new Random();
    private Vector2 nextTarget = Vector2.Zero;
    private int timer = 0;
    private const int Xmin = -10000;
    private const int Ymin = -10000;
    private const int Xmax = 10000;
    private const int Ymax = 10000;

    private Point lastTile;

    public override void _Ready()
    {
        TransitionCamera();
    }

    public override void _PhysicsProcess(float delta)
    {
        float physicsRate = 1 / delta;

        if (++timer > (physicsRate * 10))
        {
            TransitionCamera();
            timer = 0;
        }

        Position = Position.MoveToward(nextTarget, delta * speed);

        Point tile = GetTile();
        if (tile != lastTile)
        {
            EmitSignal("moved", tile.X, tile.Y, Radius);
        }

        lastTile = tile;
    }

    private Vector2 GetRandomTarget(int xmin, int xmax, int ymin, int ymax)
    {
        var possibleTarget = new Vector2(random.Next(xmin, xmax), random.Next(ymin, ymax));

        if (Position.DistanceTo(possibleTarget) > speed * timer)
            return possibleTarget;

        return GetRandomTarget(xmin, xmax, ymin, ymax);
    }

    private void TransitionCamera()
    {
        Position = GetRandomTarget(Xmin + 640, Xmax - 640, Ymin + 360, Ymax - 360);
        nextTarget = GetRandomTarget(Xmin, Xmax, Ymin, Ymax);
    }

    private Point GetTile()
    {
        return new Point((int) Position.x / 16, (int) Position.y / 16);
    }
}