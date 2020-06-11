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
    private World World;

    public override void _Ready()
    {
        World = GetNode<World>("/root/Game/World");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _PhysicsProcess(float delta)
  {
      //Text = world.GetChildCount().ToString();
      Text = $"Tiles Generated: {World.Tiles}";
  }
}