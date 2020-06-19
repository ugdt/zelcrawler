using Godot;
using zelcrawler.world.tiles;

namespace zelcrawler.levels
{
    public abstract class Level
    {
        protected readonly OpenSimplexNoise Noise = new OpenSimplexNoise();

        // World Gen Config

        public float MapPeriod { get; } = 10f;

        public int MapOctaves { get; } = 3;

        public float MapPersistence { get; } = 0.5f;

        public float MapLacunarity { get; } = 2f;

        public int MapSeed => Noise.Seed;
        
        public abstract Tile GenerateTile(int x, int y);
    }
}