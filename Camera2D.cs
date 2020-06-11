using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    [Export] private float Step = 0.2f;
    [Export] private float Min = .5f;
    [Export] private float Max = 10f;

    private Vector2 _step, _min, _max = Vector2.Zero;
    
    public override void _Ready()
    {
        _step = new Vector2(Step, Step);
        _min = new Vector2(Min, Min);
        _max = new Vector2(Max, Max);
    }

    public override void _Input(InputEvent e)
    {
        if (e.IsActionReleased("zoom_in"))
        {
            if (Zoom - _step >= _min)
            {
                Zoom -= _step;
            }
        }

        if (e.IsActionReleased("zoom_out"))
        {
            if (Zoom + _step < _max)
            {
                Zoom += _step;
            }
        }

        if (e.IsActionPressed("zoom_reset"))
        {
            Zoom = Vector2.One;
        }
    }
}