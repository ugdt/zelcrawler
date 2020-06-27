CREATE TABLE IF NOT EXISTS World (
    chunk_id INT NOT NULL,
    FOREIGN KEY(chunk_id) REFERENCES Chunk(ROWID)
);

CREATE TABLE IF NOT EXISTS Chunk (
    chunk_x INT NOT NULL,
    chunk_y INT NOT NULL,
    UNIQUE(chunk_x, chunk_y) ON CONFLICT IGNORE
);

CREATE TABLE IF NOT EXISTS ChunkToTile (
    chunk_id INT NOT NULL,
    tile_id INT NOT NULL,
    tile_x INT NOT NULL,
    tile_y INT NOT NULL,
    FOREIGN KEY(chunk_id) REFERENCES Chunk(ROWID),
    FOREIGN KEY(tile_id) REFERENCES Tile(ROWID),
    UNIQUE(tile_x, tile_y) ON CONFLICT IGNORE
);

CREATE TABLE IF NOT EXISTS Tile (
    tile_id INT NOT NULL,
    tile_name TEXT
);