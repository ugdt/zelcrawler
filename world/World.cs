using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using nextGame.levels;
using nextGame.world.tiles;

namespace nextGame.world
{
    public class World
    {
        private readonly Chunk[,] Chunks;
        private readonly int ChunkSideLength = 16;
        private readonly Level DefaultLevel;

        private readonly List<Chunk> LoadedChunks = new List<Chunk>(50);
        private readonly int WorldSize = 4096;

        public World(Level defaultLevel, TileMap tileMap = null)
        {
            Chunks = new Chunk[WorldSize, WorldSize];


            TileMap = new TileMap
            {
                Position = Vector2.Zero, CellSize = new Vector2(16, 16), TileSet = defaultLevel.TileSet
            };

            DefaultLevel = defaultLevel;
        }

        public int Tiles { get; } = 0;

        public TileMap TileMap { get; }

        private Vector2 GetChunkPos(float x, float y)
        {
            return GetChunkPos(new Vector2(x, y));
        }

        private Vector2 GetChunkPos(Vector2 worldPosition)
        {
            Vector2 mapPosition = TileMap.WorldToMap(worldPosition);

            float chunkX = (float) Math.Floor(mapPosition.x / ChunkSideLength);
            float chunkY = (float) Math.Floor(mapPosition.y / ChunkSideLength);

            return new Vector2(chunkX, chunkY);
        }

        private Vector2 ChunkPosToMap(Vector2 chunkPosition)
        {
            float mapX = chunkPosition.x * ChunkSideLength;
            float mapY = chunkPosition.y * ChunkSideLength;

            return new Vector2(mapX, mapY);
        }

        private Vector2 ChunkPosToWorld(Vector2 chunkPosition)
        {
            Vector2 worldPosition = TileMap.MapToWorld(ChunkPosToMap(chunkPosition));

            return worldPosition;
        }

        public void GenerateChunks(int x, int y)
        {
            Vector2 chunkPos = GetChunkPos(x, y);
            Chunk[] chunksAround = GetChunksAround((int) chunkPos.x, (int) chunkPos.y);

            Chunk[] chunksToGenerate = chunksAround.Except(LoadedChunks).ToArray();
            Chunk[] chunksToRemove = LoadedChunks.Except(chunksAround).ToArray();

            int gLength = chunksToGenerate.Length;
            int rLength = chunksToRemove.Length;

            int size = Math.Max(gLength, rLength);

            for (int i = 0; i < size; i++)
            {
                if (i < gLength)
                {
                    Chunk chunk = chunksToGenerate[i];
                    LoadedChunks.Add(chunk);
                    Dictionary<Tile, Vector2> chunkTiles = chunk.GetChunk();

                    foreach (Tile tile in chunkTiles.Keys)
                    {
                        Vector2 tilePos = chunkTiles[tile];
                        PlaceTile(tile, (int) tilePos.x, (int) tilePos.y);
                    }
                }

                if (i < rLength)
                {
                    Chunk chunk = chunksToRemove[i];
                    LoadedChunks.Remove(chunk);
                    Dictionary<Tile, Vector2> chunkTiles = chunk.GetChunk();

                    foreach (Vector2 tilePos in chunkTiles.Values) RemoveTile((int) tilePos.x, (int) tilePos.y);
                }
            }
        }

        private void PlaceTile(Tile tile, int mapX, int mapY)
        {
            TileMap.SetCell(mapX, mapY, tile.TileId);
        }

        private void RemoveTile(int mapX, int mapY)
        {
            TileMap.SetCell(mapX, mapY, -1);
        }

        private Chunk[] GetChunksAround(int x, int y)
        {
            List<Chunk> chunksAround = new List<Chunk>(25);

            for (int xRound = x - 2; xRound < x + 2; xRound++)
            for (int yRound = y - 2; yRound < y + 2; yRound++)
                if (xRound >= 0 && xRound < WorldSize && yRound >= 0 && yRound < WorldSize)
                {
                    if (Chunks[xRound, yRound] == null)
                        Chunks[xRound, yRound] = new Chunk(ChunkSideLength, DefaultLevel,
                            ChunkPosToMap(new Vector2(xRound, yRound)));

                    chunksAround.Add(Chunks[xRound, yRound]);
                }

            return chunksAround.ToArray();
        }
    }
}