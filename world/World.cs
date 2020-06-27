using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Godot;
using zelcrawler.levels;
using zelcrawler.world.tiles;

using File = System.IO.File;

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

		private static string _dbPath = "./testdb.sqlite";
		private static string _dbConnectionString = $"Data Source={_dbPath};Version=3;";
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
			
			InitializeDb();
		}

		private void InitializeDb()
		{
			if (!File.Exists(_dbPath))
			{
				SQLiteConnection.CreateFile(_dbPath);
				CreateTables();
			}
		}


		private void CreateTables()
		{
			using (SQLiteConnection connection = new SQLiteConnection(_dbConnectionString))
			{
				connection.Open();
				using (SQLiteTransaction transaction = connection.BeginTransaction())
				{
					string createTableQuery = File.ReadAllText("scripts/sql/createTables.sql");
					connection.Execute(createTableQuery);
					
					transaction.Commit();
				}
				connection.Close();
			}
		}

		private async Task SaveChunkToDb(IEnumerable<Chunk> chunks)
		{
			string insertChunkSql = @"INSERT INTO Chunk (chunk_x, chunk_y) VALUES (@chunkX, @chunkY); SELECT last_insert_rowid()";
			string insertTileSql = "INSERT INTO ChunkToTile (chunk_id, tile_id, tile_x, tile_y) VALUES (@chunkId, @tileId, @tileX, @tileY)";

			using (SQLiteConnection connection = new SQLiteConnection(_dbConnectionString))
			{
				connection.Open();
				using (SQLiteTransaction transaction = connection.BeginTransaction())
				{
					foreach (Chunk chunk in chunks)
					{
						Tile[,] tiles = chunk.GetTiles();

						Vector2 chunkPos = MapPosToChunk(chunk.MapPosition);
						int chunkX = (int) chunkPos.x;
						int chunkY = (int) chunkPos.y;

						int chunkId = await connection.QuerySingleAsync<int>(insertChunkSql, new {chunkX, chunkY});

						for (int x = 0; x < tiles.GetLength(0); x++)
						for (int y = 0; y < tiles.GetLength(1); y++)
						{
							Tile tile = tiles[x, y];
							int tileRowId = tile.TileId + 1; // dirty fucking hack
							await connection.ExecuteAsync(insertTileSql, new {chunkId, tileId = tileRowId, tileX = x, tileY = y});
						}
					}

					transaction.Commit();
				}
			}
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

			Task.Run(() => SaveChunkToDb(chunksToLoad));
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

			return MapPosToChunk(mapPosition);
		}

		private Vector2 ChunkPosToMap(Vector2 chunkPosition)
		{
			float mapX = chunkPosition.x * _chunkSideLength;
			float mapY = chunkPosition.y * _chunkSideLength;

			return new Vector2(mapX, mapY);
		}

		private Vector2 MapPosToChunk(Vector2 mapPosition)
		{
			float chunkX = (float) Math.Floor(mapPosition.x / _chunkSideLength);
			float chunkY = (float) Math.Floor(mapPosition.y / _chunkSideLength);

			return new Vector2(chunkX, chunkY);
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