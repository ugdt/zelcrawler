using Godot;
using System;
using System.Collections.Generic;

public class World : Node2D
{
    // Tree nodes
    Player Player;
    TileMap WorldMap;

    // Data
    private int RADIUS = 25;


    public override void _Ready()
    {
        Player = GetNode<Player>("Entities/Player");
        WorldMap = GetNode<TileMap>("WorldMap");

        GD.Print(WorldMap.TileSet.GetTilesIds());
    }

    public override void _PhysicsProcess(float delta) {
        generateTilesAroundPlayer(WorldMap.WorldToMap(Player.Position));
    }

    public void generateTilesAroundPlayer(Vector2 playerPosition) {
        //GD.Print(playerPosition);

        int playerX = (int) playerPosition.x;
        int playerY = (int) playerPosition.y;

        for (int x = playerX - RADIUS; x <= playerX; x++) {
            for (int y = playerY - RADIUS; y <= playerY; y++) {
                if (((x - playerX) ^ 2) + ((y - playerY) ^ 2) <= (RADIUS ^ 2)) {
                    int xSym = playerX - (x - playerX);
                    int ySym = playerY - (y - playerY);

                    WorldMap.SetCell(x, y, 5);
                    WorldMap.SetCell(x, ySym, 5);
                    WorldMap.SetCell(xSym, y, 5);
                    WorldMap.SetCell(xSym, ySym, 5);
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
