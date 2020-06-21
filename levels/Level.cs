using zelcrawler.world.tiles;

namespace zelcrawler.levels
{
    public abstract class Level
    {
        public abstract Tile GenerateTile(int x, int y);
        public int WorldSizeRequirement { get; protected set; } = 4096;
    }
}