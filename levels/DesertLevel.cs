using System;
using Godot;
using nextGame.world.tiles;

namespace nextGame.levels
{
    public class DesertLevel : Level
    {
        public override Tile GenerateTile(int x, int y)
        {
            float noiseVal = Noise.GetNoise2d(x, y);
            int tileCoordIndex = (int) Math.Round((noiseVal + 1) * 2.5);
            Vector2 tileCoord = new Vector2(tileCoordIndex, 0);

            if (noiseVal < 0.25f) return new GrassTile(tileCoord);

            return new DirtTile(tileCoord);
        }
    }
}