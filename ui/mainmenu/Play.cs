using Godot;
using System;

public class Play : Button
{
    public override void _Pressed() {
        base._Pressed();
        GetTree().ChangeScene("res://ui/connect/Connect.tscn");
    }
}
