using Godot;
using World = nextGame.world.World;

namespace nextGame.ui.debug
{
    public class EntityCounter : Label
    {
        private World World;

        public override void _Ready()
        {
            World = GetNode<World>("/root/Game/World");
        }

        public override void _PhysicsProcess(float delta)
        {
            Text = $"Tiles Generated: {World.GeneratedTiles}";
        }
    }
}