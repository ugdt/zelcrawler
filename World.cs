using Godot;
using System;
using System.Drawing;
using System.Collections.Generic;

public class World : Node2D
{
    // Tree nodes
    Player Player;
    TileMap WorldMap;

    // Data
    private int RADIUS = 5;
    private OpenSimplexNoise noise = new OpenSimplexNoise();


    public override void _Ready()
    {
        Player = GetNode<Player>("Entities/Player");
        WorldMap = GetNode<TileMap>("WorldMap");

        noise.Period = 10f;
    }

    public override void _PhysicsProcess(float delta)
    {
        generateTilesAroundPlayer(WorldMap.WorldToMap(Player.Position));
    }

    public void generateTilesAroundPlayer(Vector2 playerPosition)
    {
        Point playerPos = new Point((int) playerPosition.x, (int) playerPosition.y);
        Vector2 tileCoord = new Vector2(0, 0);


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
                        float noiseAtPoint = noise.GetNoise2d(point.X, point.Y);

                        if (WorldMap.GetCell(point.X, point.Y) == -1) 
                        {
                            if (noiseAtPoint < 0f)
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

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
