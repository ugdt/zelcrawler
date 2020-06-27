using System;
using Godot;

namespace zelcrawler.ui.mainmenu
{
    public class PanningCamera : Camera2D
    {
        [Signal]
        public delegate void moved(int x, int y, int radius, bool saveChunks);

        private const int Xmin = 0;
        private const int Ymin = 0;
        private const int Xmax = 65536;
        private const int Ymax = 65536;
        private readonly Random _random = new Random();

        private readonly float speed = 16 * 2;

        private Vector2 _lastPosition = Vector2.Zero;
        private Vector2 _nextTarget = Vector2.Zero;

        [Export] public int Radius = 2;
        private int _timer;

        public override void _Ready()
        {
            TransitionCamera();
        }

        public override void _PhysicsProcess(float delta)
        {
            float physicsRate = 1 / delta;

            if (++_timer > physicsRate * 10)
            {
                TransitionCamera();
                _timer = 0;
            }

            Position = Position.MoveToward(_nextTarget, delta * speed);


            if ((Position - _lastPosition).Abs().LengthSquared() > 256)
            {
                EmitSignal("moved", Position.x, Position.y, Radius, false);
                _lastPosition = Position;
            }
        }

        private Vector2 GetRandomTarget(int xmin, int xmax, int ymin, int ymax)
        {
            Vector2 possibleTarget = new Vector2(_random.Next(xmin, xmax), _random.Next(ymin, ymax));

            if (Position.DistanceTo(possibleTarget) > speed * _timer)
                return possibleTarget;

            return GetRandomTarget(xmin, xmax, ymin, ymax);
        }

        private void TransitionCamera()
        {
            Position = GetRandomTarget(Xmin + 640, Xmax - 640, Ymin + 360, Ymax - 360);
            _nextTarget = GetRandomTarget(Xmin, Xmax, Ymin, Ymax);
        }
    }
}