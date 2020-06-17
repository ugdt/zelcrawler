namespace nextGame.world.tiles
{
    public abstract class Tile
    {
        protected Tile(int tileId)
        {
            TileId = tileId;
        }

        public int TileId { get; }
    }
}