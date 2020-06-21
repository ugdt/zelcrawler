using Godot;
using zelcrawler.levels;
using zelcrawler.world.tiles;

namespace zelcrawler.world
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

        public Tile[,] GetTiles()
        {
            for (int x = 0; x < _size; x++)
            for (int y = 0; y < _size; y++)
            {
                int mapX = (int) (x + _mapPosition.x);
                int mapY = (int) (y + _mapPosition.y);

                if (_tiles[x, y] == null) _tiles[x, y] = _level.GenerateTile(mapX, mapY);
            }

            return _tiles;
        }

        public Vector2 MapPosition => _mapPosition;
        public int Size => _size;
    }
}