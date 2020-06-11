using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    
    
    private Vector2 step = new Vector2(0.10f, 0.10f);

    public override void _UnhandledInput(InputEvent e) {
                if (e.IsActionReleased("zoom_in"))
                {
                    if (Zoom - step > Vector2.Zero)
                    {
                        Zoom -= step;
                    }
                }
    
                if (e.IsActionReleased("zoom_out"))
                {
                    Zoom += step;
                }
        }
    
}
