using Godot;
using System;

public class EntityCounter : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    // public override void _Ready()
    // {
        
    // }
    private Node2D world;

    public override void _Ready() {
        base._Ready();
        world = GetNode("/root/Game") as Node2D;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta) {
      Text = world.GetChildCount().ToString();
  }
}
