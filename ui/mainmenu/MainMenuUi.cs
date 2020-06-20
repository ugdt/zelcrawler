using Godot;

namespace zelcrawler.ui.mainmenu
{
    public class MainMenuUi : Control
    {
        public void _on_Play_pressed()
        {
            GetTree().ChangeScene("res://Game.tscn");
        }

        public void _on_Settings_pressed()
        {
            GD.Print("There's no settings, play the game how we intended it noob.");
        }

        // TODO: Andrew fix this
        public override void _Input(InputEvent e)
        {
            if (e.IsActionPressed("fullscreen")) OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }
}