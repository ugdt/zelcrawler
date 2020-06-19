using Godot;
using zelcrawler.world;
using World = zelcrawler.world.World;

namespace zelcrawler.ui.debug
{
    public class TileCounter : Label
    {
        private World _world;

        public override void _Ready()
        {
            _world = GetNode<World>("/root/Game/World");
        }

        public override void _PhysicsProcess(float delta)
        {
            Text = $"Tiles Generated: {_world.GeneratedTiles}";
        }
    }
}