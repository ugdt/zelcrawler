using Godot;
using System;
using System.Drawing;

public class World : Node2D
{
    // Tree nodes
    private TileMap WorldMap;

    // Data
    private readonly OpenSimplexNoise noise = new OpenSimplexNoise();
    private static int _tiles = 0;

    // World Gen Config
    [Export] private float Period = 10f;
    [Export] private int Octaves = 3;
    [Export] private float Persistence = 0.5f;
    [Export] private float Lacunarity = 2f;

    public override void _Ready()
    {
        WorldMap = GetNode<TileMap>("WorldMap");

        Random random = new Random();

        noise.Period = Period;
        noise.Octaves = Octaves;
        noise.Persistence = Persistence;
        noise.Lacunarity = Lacunarity;
        noise.Seed = random.Next() % 10;
    }

    public override void _Input(InputEvent e)
    {
        if (e.IsActionPressed("fullscreen"))
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }

    public void _on_moved(int x, int y, int radius)
    {
        GenerateTiles(new Point(x, y), radius);
    }

    private void GenerateTiles(Point position, int radius)
    {
        Vector2 tileCoords = Vector2.Zero;

        for (int x = (position.X - radius); x <= position.X; x++)
        {
            for (int y = (position.Y - radius); y <= position.Y; y++)
            {
                if (((x - position.X) * (x - position.X)) + ((y - position.Y) * (y - position.Y)) <= (radius * radius))
                {
                    int xSym = position.X - (x - position.X);
                    int ySym = position.Y - (y - position.Y);

                    Point[] points = {new Point(x, y), new Point(xSym, y), new Point(x, ySym), new Point(xSym, ySym)};

                    foreach (Point point in points)
                    {
                        if (WorldMap.GetCell(point.X, point.Y) == -1)
                        {
                            float noiseVal = noise.GetNoise2d(point.X, point.Y);

                            int index = (int) Math.Round((noiseVal + 1) * 2.5);
                            tileCoords.x = index;

                            if (noiseVal < 0.25f)
                            {
                                WorldMap.SetCell(point.X, point.Y, 0, autotileCoord: tileCoords);
                            }
                            else
                            {
                                WorldMap.SetCell(point.X, point.Y, 1, autotileCoord: tileCoords);
                            }

                            _tiles++;
                        }
                    }
                }
            }
        }
    }

    public void GenerateTiles(Vector2 position, int radius)
    {
        Vector2 positionOnMap = WorldMap.MapToWorld(position);
        Point point = new Point((int) positionOnMap.x, (int) positionOnMap.y);
    }


    public float MapPeriod => Period;

    public int MapOctaves => Octaves;

    public float MapPersistence => Persistence;

    public float MapLacunarity => Lacunarity;

    public int MapSeed => noise.Seed;

    public int Tiles => _tiles;
}