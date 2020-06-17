using System.Collections.Generic;
using Godot;
using nextGame.levels;
using nextGame.world.tiles;

namespace nextGame.world
{
    public class Chunk
    {
        private readonly Level Level;
        private readonly Vector2 MapPosition;
        private readonly int Size;
        private readonly Tile[,] Tiles;

        public Chunk(int size, Level level, Vector2 mapPosition)
        {
            Tiles = new Tile[size, size];
            Size = size;
            Level = level;
            MapPosition = mapPosition;
        }

        public Dictionary<Tile, Vector2> GetChunk()
        {
            Dictionary<Tile, Vector2> tiles = new Dictionary<Tile, Vector2>(Size * Size);

            for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
            {
                int mapX = (int) (x + MapPosition.x);
                int mapY = (int) (y + MapPosition.y);

                if (Tiles[x, y] == null) Tiles[x, y] = Level.GenerateTile(mapX, mapY);

                Tile tile = Tiles[x, y];
                tiles.Add(tile, new Vector2(mapX, mapY));
            }

            return tiles;
        }
    }
}