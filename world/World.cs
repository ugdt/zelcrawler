using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;
using nextGame.levels;
using nextGame.world.tiles;

namespace nextGame.world
{
	public class World : Node2D
	{
		private Level _currentLevel;
		private TileMap _worldMap;
		
		private ChunkQueue _chunkQueue;
		private Chunk[,] _chunks;
		[Export] private int _chunkSideLength = 16;
		[Export] private int _worldSize = 16384;
		private int DefaultRadius = 2;

		public int GeneratedTiles { get; private set; } = 0;

		public override void _Ready()
		{
			_chunks = new Chunk[_worldSize, _worldSize];
			_worldMap = GetNode<TileMap>("WorldMap");
			_currentLevel = new TutorialLevel();
			_chunkQueue = new ChunkQueue(32);
		}

		private Vector2 GetChunkPos(float x, float y)
		{
			return GetChunkPos(new Vector2(x, y));
		}

		private Vector2 GetChunkPos(Vector2 worldPosition)
		{
			Vector2 mapPosition = _worldMap.WorldToMap(worldPosition);

			float chunkX = (float) Math.Floor(mapPosition.x / _chunkSideLength);
			float chunkY = (float) Math.Floor(mapPosition.y / _chunkSideLength);

			return new Vector2(chunkX, chunkY);
		}

		private Vector2 ChunkPosToMap(Vector2 chunkPosition)
		{
			float mapX = chunkPosition.x * _chunkSideLength;
			float mapY = chunkPosition.y * _chunkSideLength;

			return new Vector2(mapX, mapY);
		}

		private void GenerateChunks(int worldX, int worldY, int radius)
		{
			Vector2 chunkPos = GetChunkPos(worldX, worldY);
			Chunk[] chunksAround = GetChunksAround((int) chunkPos.x, (int) chunkPos.y, radius);
			
			ReadOnlyCollection<Chunk> loadedChunks = _chunkQueue.LoadedChunks;
			Chunk[] chunksToLoad = chunksAround.Except(loadedChunks).ToArray();
			Chunk[] chunksToFallBehind = loadedChunks.Intersect(chunksAround).ToArray();

			int numChunksToLoad = chunksToLoad.Length;
			int numChunksToFallBehind = chunksToFallBehind.Length;

			int loops = Math.Max(numChunksToLoad, numChunksToFallBehind);

			for (int i = 0; i < loops; i++)
			{
				if (numChunksToLoad > i)
				{
					LoadChunk(chunksToLoad[i]);
				}

				if (numChunksToFallBehind > i)
				{
					_chunkQueue.FallBehind(chunksToFallBehind[i]);
				}
			}
		}

		private void LoadChunk(Chunk chunk)
		{
			bool unloadAChunk = _chunkQueue.Enqueue(chunk);
			
			Dictionary<Tile, Vector2> chunkTiles = chunk.GetChunk();

			foreach (Tile tile in chunkTiles.Keys)
			{
				Vector2 tilePos = chunkTiles[tile];
				PlaceTile(tile, (int) tilePos.x, (int) tilePos.y);
			}

			if (unloadAChunk)
			{
				UnloadChunk(_chunkQueue.Dequeue());
			}
		}

		private void UnloadChunk(Chunk chunk)
		{
			Dictionary<Tile, Vector2> chunkTiles = chunk.GetChunk();
			foreach (Vector2 tilePos in chunkTiles.Values) RemoveTile((int) tilePos.x, (int) tilePos.y);
		}

		private void PlaceTile(Tile tile, int mapX, int mapY)
		{
			_worldMap.SetCell(mapX, mapY, tile.TileId, autotileCoord: tile.TileCoordinate);
			GeneratedTiles++;
		}

		private void RemoveTile(int mapX, int mapY)
		{
			_worldMap.SetCell(mapX, mapY, -1);
		}

		private Chunk[] GetChunksAround(int x, int y, int radius)
		{
			int diameter = radius + radius + 1;
			List<Chunk> chunksAround = new List<Chunk>(diameter * diameter);

			for (int xRound = x - radius; xRound < x + radius; xRound++)
			for (int yRound = y - radius; yRound < y + radius; yRound++)
				if (xRound >= 0 && xRound < _worldSize && yRound >= 0 && yRound < _worldSize)
				{
					if (_chunks[xRound, yRound] == null)
						_chunks[xRound, yRound] = new Chunk(_chunkSideLength, _currentLevel,
							ChunkPosToMap(new Vector2(xRound, yRound)));

					chunksAround.Add(_chunks[xRound, yRound]);
				}

			return chunksAround.ToArray();
		}

		public void _moved(int worldX, int worldY, int radius = -1)
		{
			if (radius < 1)
			{
				radius = DefaultRadius;
			}
			
			GenerateChunks(worldX, worldY, radius);
		}
	}
}
