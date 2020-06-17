using nextGame.world.tiles;

namespace nextGame.levels
{
    public class TutorialLevel : Level
    {
        public override Tile GenerateTile(int x, int y)
        {
            float noiseVal = Noise.GetNoise2d(x, y);

            if (noiseVal < 0.25f) return new GrassTile();

            return new DirtTile();
        }
    }
}