using Godot;

namespace nextGame
{
    public class Game : Node2D
    {
        public override void _Input(InputEvent e)
        {
            if (e.IsActionPressed("fullscreen")) OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }
}