using Godot;
using System;
using System.Drawing;

public class World : Node2D
{
    // Tree nodes
    Player Player;
    TileMap WorldMap;

    // Data
    private int RADIUS = 10;
    private OpenSimplexNoise noise = new OpenSimplexNoise();
    private int tiles = 0;

    // World Gen Config
    [Export] private float Period = 10f;
    [Export] private int Octaves = 3;
    [Export] private float Persistence = 0.5f;
    [Export] private float Lacunarity = 2f;

    public float MapPeriod {
        get { return Period; }
    }

    public int MapOctaves {
        get { return Octaves; }
    }

    public float MapPersistence {
        get { return Persistence; }
    }

    public float MapLacunarity {
        get { return Lacunarity; }
    }

    public int MapSeed {
        get { return noise.Seed; }
    }

    public int Tiles {
        get { return tiles; }
    }

    public override void _Ready()
    {
        Player = GetNode<Player>("Entities/Player");
        WorldMap = GetNode<TileMap>("WorldMap");

        Random random = new Random();

        noise.Period = Period;
        noise.Octaves = Octaves;
        noise.Persistence = Persistence;
        noise.Lacunarity = Lacunarity;
        noise.Seed = random.Next() % 10;
    }

    public override void _PhysicsProcess(float delta)
    {
        generateTiles(WorldMap.WorldToMap(Player.Position));
    }

    public void generateTiles(Vector2 position)
    {
        Point originalPoint = new Point((int) position.x, (int) position.y);
        Vector2 tileCoord = Vector2.Zero;

        for (int x = (originalPoint.X - RADIUS); x <= originalPoint.X; x++)
        {
            for (int y = (originalPoint.Y - RADIUS); y <= originalPoint.Y; y++)
            {
                if (((x - originalPoint.X) * (x - originalPoint.X)) + ((y - originalPoint.Y) * (y - originalPoint.Y)) <= (RADIUS * RADIUS))
                {
                    int mirroredX = originalPoint.X - (x - originalPoint.X);
                    int mirroredY = originalPoint.Y - (y - originalPoint.Y);

                    Point[] points = { new Point(x, y), new Point(mirroredX, y), new Point(x, mirroredY), new Point(mirroredX, mirroredY) };

                    foreach (Point point in points)
                    {
                        if (WorldMap.GetCell(point.X, point.Y) == -1) 
                        {
                            float noiseVal = noise.GetNoise2d(point.X, point.Y);

                            int index = (int) Math.Round((noiseVal + 1) * 2.5);
                            tileCoord.x = index;

                            if (noiseVal < 0.25f)
                            {
                                WorldMap.SetCell(point.X, point.Y, 0, autotileCoord: tileCoord);
                            }
                            else
                            {
                                WorldMap.SetCell(point.X, point.Y, 1, autotileCoord: tileCoord);
                            }
                            tiles++;
                        }
                    }
                }
            }
        }

    }
}
