using Godot;

namespace zelcrawler.world.tiles
{
    public abstract class Tile
    {
        protected Tile(int tileId, Vector2 tileCoordinate)
        {
            TileId = tileId;
            TileCoordinate = tileCoordinate;
        }

        public int TileId { get; }
        public Vector2 TileCoordinate { get; }
    }
}