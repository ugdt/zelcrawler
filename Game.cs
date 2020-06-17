using Godot;
using nextGame.levels;
using World = nextGame.world.World;

namespace nextGame
{
    public class Game : Node2D
    {
        private World CurrentWorld;

        public override void _Ready()
        {
            CurrentWorld = new World(new DesertLevel());
            Node2D gameWorld = GetNode<Node2D>("World");
            gameWorld.AddChild(CurrentWorld.TileMap);
        }

        public override void _Input(InputEvent e)
        {
            if (e.IsActionPressed("fullscreen")) OS.WindowFullscreen = !OS.WindowFullscreen;
        }

        private void SwapWorlds(World newWorld)
        {
            RemoveChild(CurrentWorld.TileMap);
            AddChild(newWorld.TileMap);
            CurrentWorld = newWorld;
        }

        public void _on_moved(int x, int y)
        {
            CurrentWorld.GenerateChunks(x, y);
        }
    }
}