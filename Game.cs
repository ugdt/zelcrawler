using Godot;
using System;

public class Game : Node2D
{
    public override void _Ready()
    {
        
    }
    
    public override void _UnhandledInput(InputEvent e) {
            if (e.IsActionReleased("zoom_in")) {
            }

            if (e.IsActionReleased("zoom_out"))
            {
            }
    }
}
