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
    private OpenSimplexNoise noiseOne = new OpenSimplexNoise();
    private OpenSimplexNoise noiseTwo = new OpenSimplexNoise();

    public override void _Ready()
    {
        Player = GetNode<Player>("Entities/Player");
        WorldMap = GetNode<TileMap>("WorldMap");

        Random random = new Random();

        noiseOne.Period = 10f;
        noiseOne.Seed = random.Next() % 10;
        noiseTwo.Period = 10f;
        noiseTwo.Seed = random.Next() % 10;
    }

    public override void _PhysicsProcess(float delta)
    {
        generateTilesAroundPlayer(WorldMap.WorldToMap(Player.Position));
    }

    public void generateTilesAroundPlayer(Vector2 playerPosition)
    {
        Point playerPos = new Point((int) playerPosition.x, (int) playerPosition.y);
        Vector2 tileCoord = Vector2.Zero;


        for (int x = playerPos.X - RADIUS; x <= playerPos.X; x++)
        {
            for (int y = playerPos.Y - RADIUS; y <= playerPos.Y; y++)
            {
                if (((x - playerPos.X) ^ 2) + ((y - playerPos.Y) ^ 2) <= (RADIUS ^ 2))
                {
                    int xSym = playerPos.X - (x - playerPos.X);
                    int ySym = playerPos.Y - (y - playerPos.Y);

                    Point[] points = { new Point(x, y), new Point(xSym, y), new Point(x, ySym), new Point(xSym, ySym) };

                    foreach (Point point in points)
                    {
                        float noiseBase = noiseOne.GetNoise2d(point.X, point.Y);
                        float noiseExtra = noiseTwo.GetNoise2d(point.X, point.Y);

                        if (WorldMap.GetCell(point.X, point.Y) == -1) 
                        {
                            int index = (int) Math.Round((noiseExtra + 1) * 2.5);
                            tileCoord.x = index;

                            if (noiseBase < 0.25f)
                            {
                                WorldMap.SetCell(point.X, point.Y, 0, autotileCoord: tileCoord);
                            } 
                            else 
                            {
                                WorldMap.SetCell(point.X, point.Y, 1, autotileCoord: tileCoord);
                            }
                        }

                    }
                }
            }
        }
    }
}
