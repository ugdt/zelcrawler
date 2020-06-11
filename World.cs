using Godot;
using System;
using System.Drawing;

public class World : Node2D
{
    // Tree nodes
    private TileMap WorldMap;

    // Data
    private int RADIUS = 10;
    private OpenSimplexNoise noise = new OpenSimplexNoise();
    private int tiles = 0;

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

    public override void _PhysicsProcess(float delta)
    {
        // generateTilesAroundPlayer(WorldMap.WorldToMap(Position));
    }
    
    public override void _Input(InputEvent e) {
            if (e.IsActionPressed("fullscreen"))
            {
                OS.WindowFullscreen = !OS.WindowFullscreen;
            }
        }
    
    public void _on_moved(int x, int y, int radius)
        {
            generateTiles(new Point(x, y), radius);
        }

    public void generateTiles(Point position, int radius)
    {
        Vector2 tileCoord = Vector2.Zero;


        for (int x = (position.X - radius); x <= position.X; x++)
        {
            for (int y = (position.Y - radius); y <= position.Y; y++)
            {
                if (((x - position.X) * (x - position.X)) + ((y - position.Y) * (y - position.Y)) <= (radius * radius))
                {
                    int xSym = position.X - (x - position.X);
                    int ySym = position.Y - (y - position.Y);

                    Point[] points = { new Point(x, y), new Point(xSym, y), new Point(x, ySym), new Point(xSym, ySym) };

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
}
