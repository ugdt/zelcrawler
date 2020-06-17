using Godot;
using nextGame.world.tiles;

namespace nextGame.levels
{
    public abstract class Level
    {
        protected readonly OpenSimplexNoise Noise = new OpenSimplexNoise();
        protected string TileSetPath;

        // World Gen Config

        public float MapPeriod { get; } = 10f;

        public int MapOctaves { get; } = 3;

        public float MapPersistence { get; } = 0.5f;

        public float MapLacunarity { get; } = 2f;

        public int MapSeed => Noise.Seed;

        public TileSet TileSet => ResourceLoader.Load<TileSet>(TileSetPath);

        public abstract Tile GenerateTile(int x, int y);
    }
}