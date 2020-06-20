using Godot;

namespace zelcrawler.ui.debug
{
    public class FpsCounter : Label
    {
        public override void _Ready()
        {
            Text = "0";
        }

        public override void _Process(float delta)
        {
            Text = "FPS: " + Engine.GetFramesPerSecond();
        }
    }
}