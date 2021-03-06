using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;
using zelcrawler.levels;
using zelcrawler.world.tiles;

namespace zelcrawler.world
{
	public class World : Node2D
	{
		private Level _currentLevel;
		private TileMap _worldMap;

		[Export] private LevelEnum _defaultLevel = LevelEnum.Tutorial;
		private ChunkQueue _chunkQueue;
		[Export] private int _chunkSideLength = 16;
		private int DefaultRadius = 2;
		private Dictionary<Vector2, Chunk> _chunks;
		public int GeneratedTiles { get; private set; }

		public override void _Ready()
		{
			_worldMap = GetNode<TileMap>("WorldMap");

			switch (_defaultLevel)
			{
				case LevelEnum.Tutorial:
					_currentLevel = new TutorialLevel();
					break;
				case LevelEnum.Desert:
					_currentLevel = new DesertLevel();
					break;
			}

			_chunks = new Dictionary<Vector2, Chunk>(_currentLevel.WorldSizeRequirement);
			_chunkQueue = new ChunkQueue(50);
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
			
			Tile[,] chunkTiles = chunk.GetTiles();

			for (int x = 0; x < chunk.Size; x++)
			for (int y = 0; y < chunk.Size; y++)
			{
				int mapX = (int) (x + chunk.MapPosition.x);
				int mapY = (int) (y + chunk.MapPosition.y);

				PlaceTile(chunkTiles[x, y], mapX, mapY);
			}

			if (unloadAChunk)
			{
				UnloadChunk(_chunkQueue.Dequeue());
			}
		}

		private void UnloadChunk(Chunk chunk)
		{
			for (int x = 0; x < chunk.Size; x++)
			for (int y = 0; y < chunk.Size; y++)
			{
				int mapX = (int) (x + chunk.MapPosition.x);
				int mapY = (int) (y + chunk.MapPosition.y);

				RemoveTile(mapX, mapY);
			}
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
			{
				Vector2 chunkPos = new Vector2(xRound, yRound);
				
				if (! _chunks.ContainsKey(chunkPos))
					_chunks[chunkPos] = new Chunk(_chunkSideLength, _currentLevel,
						ChunkPosToMap(chunkPos));

				chunksAround.Add(_chunks[chunkPos]);
			}

			return chunksAround.ToArray();
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
