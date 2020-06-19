using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace nextGame.world
{
    public class ChunkQueue
    {
        private readonly int _maxSize;
        private readonly List<Chunk> _currentChunks;

        public ChunkQueue(int maxSize)
        {
            _currentChunks = new List<Chunk>(maxSize + 1);
            _maxSize = maxSize;
        }

        public bool Enqueue(Chunk chunk)
        {
            _currentChunks.Add(chunk);
            return _currentChunks.Count == _maxSize;
        }

        public Chunk Dequeue()
        {
            Chunk chunk = _currentChunks[0];
            _currentChunks.RemoveAt(0);
            return chunk;
        }

        public void FallBehind(Chunk chunk)
        {
            if (_currentChunks.Remove(chunk))
            {
                Enqueue(chunk);
            }
        }

        public ReadOnlyCollection<Chunk> LoadedChunks => _currentChunks.AsReadOnly();
    }
}