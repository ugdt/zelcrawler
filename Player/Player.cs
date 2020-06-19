using System;
using Godot;
using static nextGame.Player.PDirection;

namespace nextGame.Player
{
    public class Player : KinematicBody2D
    {
        [Signal]
        public delegate void moved(int x, int y, int radius);

        [Export] private int _acceleration = 500;

        private AnimationTree _aTree;
        private AnimationNodeStateMachinePlayback _aTreePlayback;

        [Export] private int _worldGenRadius = 2;
        private float _currentSpeed;
        [Export] private int _friction = 500;
        private bool _isMoving;
        private Vector2 _lastPosition = Vector2.Zero;

        [Export] private int _maxSpeed = 75;
        private PDirection _playerDirection = Right;
        private Sword _sword;
        private Vector2 _velocity = Vector2.Zero;

        public override void _Ready()
        {
            Position = new Vector2(32768, 32768);
            _aTree = GetNode<AnimationTree>("AnimationTree");
            _sword = GetNode<Sword>("Sword");
            _aTreePlayback =
                (AnimationNodeStateMachinePlayback) _aTree.Get("parameters/AnimationNodeStateMachine/playback");
            _lastPosition = Vector2.Zero;
        }

        private Vector2 GetInput(float delta)
        {
            Vector2 inputVelocity = Vector2.Zero;

            if (!_sword.Swinging)
            {
                inputVelocity.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
                inputVelocity.y = Input.GetActionStrength("down") - Input.GetActionStrength("up");

                inputVelocity = inputVelocity.Normalized();
            }

            if (inputVelocity == Vector2.Zero)
                _isMoving = false;
            else
                _isMoving = true;


            if (_isMoving)
            {
                _aTreePlayback.Travel("Running");
                _aTree.Set("parameters/AnimationNodeStateMachine/Idle/blend_position", inputVelocity);
                _aTree.Set("parameters/AnimationNodeStateMachine/Running/blend_position", inputVelocity);

                _currentSpeed += _acceleration * delta;

                if (_currentSpeed > _maxSpeed) _currentSpeed = _maxSpeed;

                float animationSpeed = 1 + 2 * (_currentSpeed / _maxSpeed);

                _aTree.Set("parameters/TimeScale/scale", animationSpeed);

                return _velocity.MoveToward(inputVelocity * _currentSpeed, _acceleration * delta);
            }

            _aTreePlayback.Travel("Idle");
            _aTree.Set("parameter/playback", "Idle");

            _currentSpeed = 0;

            return _velocity.MoveToward(Vector2.Zero, _friction * delta);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Input.IsActionPressed("up")) _playerDirection = Up;

            if (Input.IsActionPressed("down")) _playerDirection = Down;

            if (Input.IsActionPressed("left")) _playerDirection = Left;

            if (Input.IsActionPressed("right")) _playerDirection = Right;

            _velocity = MoveAndSlide(GetInput(delta));

            if (_isMoving)
            {
            }

            if (!_sword.Swinging)
                switch (_playerDirection)
                {
                    case Up:
                        _sword.Rotation = 0f;
                        _sword.Position = new Vector2(0, -15);
                        break;
                    case Down:
                        _sword.Rotation = Convert.ToSingle(Math.PI);
                        _sword.Position = new Vector2(0, 15);
                        break;
                    case Left:
                        _sword.Rotation = Convert.ToSingle(3 * Math.PI / 2);
                        _sword.Position = new Vector2(-16, -5);
                        break;
                    case Right:
                        _sword.Rotation = Convert.ToSingle(Math.PI / 2d);
                        _sword.Position = new Vector2(16, -5);
                        break;
                }

            if ((Position - _lastPosition).Abs().LengthSquared() > 256)
            {
                EmitSignal("moved", Position.x, Position.y, _worldGenRadius);
                _lastPosition = Position;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("slash")) _sword.Swing();
        }
    }
}