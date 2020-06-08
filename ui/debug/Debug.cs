using Godot;
using System;

public class Debug : Control {
    public override void _Ready() {
        Visible = false;
    }

    public override void _UnhandledInput(InputEvent e) {
        if (e.IsActionReleased("show_debug")) {
            Visible = !Visible;
        }
    }
}