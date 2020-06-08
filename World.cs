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

    public override void _PhysicsProcess(float delta) {
        generateTilesAroundPlayer(WorldMap.WorldToMap(Player.Position));
    }

    public void generateTilesAroundPlayer(Vector2 playerPosition) {
        Point playerPos = new Point((int) playerPosition.x, (int) playerPosition.y);

        Vector2 tileCoord = new Vector2(0, 0);


        for (int x = playerPos.X - RADIUS; x <= playerPos.X; x++) {
            for (int y = playerPos.Y - RADIUS; y <= playerPos.Y; y++) {
                if (((x - playerPos.X) ^ 2) + ((y - playerPos.Y) ^ 2) <= (RADIUS ^ 2)) {
                    int xSym = playerPos.X - (x - playerPos.X);
                    int ySym = playerPos.Y - (y - playerPos.Y);

                    float valOne = noise.GetNoise2d(x, y);
                    float valTwo = noise.GetNoise2d(x, ySym);
                    float valThree = noise.GetNoise2d(xSym, y);
                    float valFour = noise.GetNoise2d(xSym, ySym);

                    if (valOne < 0f)
                    {
                        WorldMap.SetCell(x, y, 0, autotileCoord: tileCoord);
                    }
                    else
                    {
                        WorldMap.SetCell(x, y, 1, autotileCoord: tileCoord);
                    }

                    if (valTwo < 0f)
                    {
                        WorldMap.SetCell(x, ySym, 0, autotileCoord: tileCoord);
                    }
                    else
                    {
                        WorldMap.SetCell(x, ySym, 1, autotileCoord: tileCoord);
                    }

                    if (valThree < 0f)
                    {
                        WorldMap.SetCell(xSym, y, 0, autotileCoord: tileCoord);
                    }
                    else
                    {
                        WorldMap.SetCell(xSym, y, 1, autotileCoord: tileCoord);
                    }

                    if (valFour < 0f)
                    {
                        WorldMap.SetCell(xSym, ySym, 0, autotileCoord: tileCoord);
                    }
                    else
                    {
                        WorldMap.SetCell(xSym, ySym, 1, autotileCoord: tileCoord);
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
