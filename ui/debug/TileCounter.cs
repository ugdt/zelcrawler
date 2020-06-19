using Godot;

namespace nextGame.ui.debug
{
    public class TileCounter : Label
    {
        private nextGame.world.World _world;

        public override void _Ready()
        {
            _world = GetNode<nextGame.world.World>("/root/Game/World");
        }

        public override void _PhysicsProcess(float delta)
        {
            Text = $"Tiles Generated: {_world.GeneratedTiles}";
        }
    }
}