namespace nextGame.world
{
    public class Chunk
    {

        private Tile[,] _tiles;
        private Level _level;

        public Chunk(int size, Level level)
        {
            _tiles = new Tile[size,size];
            _level = level;
        }

        private void generateChunk()
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
            }
        }
    }
}