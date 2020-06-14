using Godot;
using System;

public class TileCounter : Label
{
    private Level Level;

    public override void _Ready()
    {
        Level = GetNode<Level>("/root/Game/Level");
    }

    public override void _PhysicsProcess(float delta)
    {
        Text = $"Tiles {Level.Tiles}";
    }
}