using Godot;

namespace zelcrawler
{
    public class Game : Node2D
    {
        public override void _Input(InputEvent e)
        {
            if (e.IsActionPressed("fullscreen")) OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }
}