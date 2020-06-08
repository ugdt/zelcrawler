using Godot;
using System;
using System.Diagnostics;

public class PanningCamera : Camera2D {

    private float speed = 24 * 2;
    private Random random = new Random();
    private Vector2 nextTarget = Vector2.Zero;
    private float timer;
    private int xmin, xmax, ymin, ymax;

    public override void _Ready() {
        base._Ready();
        xmin = ymin = 0;
        xmax = ymax = 2400;
    }

    public override void _Process(float delta) {
        base._Process(delta);
        timer += delta;
        
        if (timer > 10f) {
            transitionCamera();
            timer = 0f;
        }

        Position = Position.MoveToward(nextTarget, delta * speed);
    }

    public Vector2 getRandomTarget(int xmin, int xmax, int ymin, int ymax) {
        var possibleTarget = new Vector2(random.Next(xmin, xmax), random.Next(ymin, ymax));
        
        if (Position.DistanceTo(possibleTarget) > speed * timer)
            return possibleTarget;
        
        return getRandomTarget(xmin, xmax, ymin, ymax);
    }

    public void transitionCamera() {
        // Position = getRandomTarget(xmin + (int) OS.WindowSize.x / 2, xmax , ymin, ymax - (int) OS.WindowSize.y);
        Position = getRandomTarget(xmin + 640, xmax - 640, ymin + 360, ymax - 360);
        nextTarget = getRandomTarget(xmin, xmax, ymin, ymax);
        GD.Print(Position);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
