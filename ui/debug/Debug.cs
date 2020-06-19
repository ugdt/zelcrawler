#region

using Godot;

#endregion

namespace zelcrawler.ui.debug
{
    public class Debug : Control
    {
        public override void _Ready()
        {
            Visible = false;
        }

        public override void _Input(InputEvent e)
        {
            if (e.IsActionReleased("show_debug")) Visible = !Visible;
        }
    }
}