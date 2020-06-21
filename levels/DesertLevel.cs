using System;
using Godot;
using zelcrawler.world.tiles;

namespace zelcrawler.levels
{
    public class DesertLevel : Level
    {
        private readonly OpenSimplexNoise _noise = new OpenSimplexNoise();

        public override Tile GenerateTile(int x, int y)
        {
            float noiseVal = _noise.GetNoise2d(x, y);
            int tileCoordinateIndex = (int) Math.Round((noiseVal + 1) * 2.5);
            Vector2 tileCoordinate = new Vector2(tileCoordinateIndex, 0);

            if (noiseVal < 0.25f) return new GrassTile(tileCoordinate);

            return new DirtTile(tileCoordinate);
        }
    }
}