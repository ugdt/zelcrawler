using Godot;
using System;

public class TileCounter : Label
{
    private World World;

    public override void _Ready()
    {
        World = GetNode<World>("/root/Game/World");
    }

  public override void _PhysicsProcess(float delta)
  {
      Text = $"Tiles {World.Tiles}";
  }
}
