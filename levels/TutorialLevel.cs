using System;
using Godot;
using nextGame.world.tiles;

namespace nextGame.levels
{
    public class TutorialLevel : Level
    {
        public override Tile GenerateTile(int x, int y)
        {
            float noiseVal = Noise.GetNoise2d(x, y);
            int tileCoordinateIndex = (int) Math.Round((noiseVal + 1) * 2.5);
            Vector2 tileCoordinate = new Vector2(tileCoordinateIndex, 0);

            if (noiseVal > -0.25f) return new GrassTile(tileCoordinate);

            return new DirtTile(tileCoordinate);
        }
    }
}