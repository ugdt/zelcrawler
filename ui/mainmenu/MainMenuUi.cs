using Godot;
using System;

public class MainMenuUi : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Play_pressed()
    {
        GetTree().ChangeScene("res://Game.tscn");
    }

    public void _on_Settings_pressed()
    {
        GD.Print("There's no settings, play the game how we intended it noob.");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
