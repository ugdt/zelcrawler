using System.Collections.Generic;
using Godot;
using nextGame.levels;
using nextGame.world.tiles;

namespace nextGame.world
{
    public class Chunk
    {
        private readonly Level _level;
        private readonly Vector2 _mapPosition;
        private readonly int _size;
        private readonly Tile[,] _tiles;

        public Chunk(int size, Level level, Vector2 mapPosition)
        {
            _tiles = new Tile[size, size];
            _size = size;
            _level = level;
            _mapPosition = mapPosition;
        }

        public Dictionary<Tile, Vector2> GetChunk()
        {
            Dictionary<Tile, Vector2> tiles = new Dictionary<Tile, Vector2>(_size * _size);

            for (int x = 0; x < _size; x++)
            for (int y = 0; y < _size; y++)
            {
                int mapX = (int) (x + _mapPosition.x);
                int mapY = (int) (y + _mapPosition.y);

                if (_tiles[x, y] == null) _tiles[x, y] = _level.GenerateTile(mapX, mapY);

                Tile tile = _tiles[x, y];
                tiles.Add(tile, new Vector2(mapX, mapY));
            }

            return tiles;
        }
    }
}