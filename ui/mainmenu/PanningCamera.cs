using System;
using Godot;

namespace nextGame.ui.mainmenu
{
    public class PanningCamera : Camera2D
    {
        [Signal]
        public delegate void moved(int x, int y);

        private const int Xmin = 0;
        private const int Ymin = 0;
        private const int Xmax = 65536;
        private const int Ymax = 65536;
        private readonly Random random = new Random();

        private readonly float speed = 16 * 2;

        private Vector2 lastPosition = Vector2.Zero;
        private Vector2 nextTarget = Vector2.Zero;

        [Export] public int Radius = 15;
        private int timer;

        public override void _Ready()
        {
            TransitionCamera();
        }

        public override void _PhysicsProcess(float delta)
        {
            float physicsRate = 1 / delta;

            if (++timer > physicsRate * 10)
            {
                TransitionCamera();
                timer = 0;
            }

            Position = Position.MoveToward(nextTarget, delta * speed);


            if ((Position - lastPosition).Abs().LengthSquared() > 256)
            {
                EmitSignal("moved", Position.x, Position.y);
                lastPosition = Position;
            }
        }

        private Vector2 GetRandomTarget(int xmin, int xmax, int ymin, int ymax)
        {
            Vector2 possibleTarget = new Vector2(random.Next(xmin, xmax), random.Next(ymin, ymax));

            if (Position.DistanceTo(possibleTarget) > speed * timer)
                return possibleTarget;

            return GetRandomTarget(xmin, xmax, ymin, ymax);
        }

        private void TransitionCamera()
        {
            Position = GetRandomTarget(Xmin + 640, Xmax - 640, Ymin + 360, Ymax - 360);
            nextTarget = GetRandomTarget(Xmin, Xmax, Ymin, Ymax);
        }
    }
}