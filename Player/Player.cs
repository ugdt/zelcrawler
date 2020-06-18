using System;
using Godot;
using static nextGame.Player.PDirection;

namespace nextGame.Player
{
    public class Player : KinematicBody2D
    {
        [Signal]
        public delegate void moved(int x, int y);

        [Export] private int Acceleration = 500;

        private AnimationTree ATree;
        private AnimationNodeStateMachinePlayback ATreePlayback;

        private float CurrentSpeed;
        [Export] private int Friction = 500;
        private bool IsMoving;
        private Vector2 lastPosition = Vector2.Zero;

        [Export] private int MaxSpeed = 75;
        private PDirection PlayerDirection = Right;
        private Sword Sword;
        private Vector2 Velocity = Vector2.Zero;

        public override void _Ready()
        {
            Position = new Vector2(32768, 32768);
            ATree = GetNode<AnimationTree>("AnimationTree");
            Sword = GetNode<Sword>("Sword");
            ATreePlayback =
                (AnimationNodeStateMachinePlayback) ATree.Get("parameters/AnimationNodeStateMachine/playback");
            lastPosition = Vector2.Zero;
        }

        private Vector2 GetInput(float delta)
        {
            Vector2 inputVelocity = Vector2.Zero;

            if (!Sword.Swinging)
            {
                inputVelocity.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
                inputVelocity.y = Input.GetActionStrength("down") - Input.GetActionStrength("up");

                inputVelocity = inputVelocity.Normalized();
            }

            if (inputVelocity == Vector2.Zero)
                IsMoving = false;
            else
                IsMoving = true;


            if (IsMoving)
            {
                ATreePlayback.Travel("Running");
                ATree.Set("parameters/AnimationNodeStateMachine/Idle/blend_position", inputVelocity);
                ATree.Set("parameters/AnimationNodeStateMachine/Running/blend_position", inputVelocity);

                CurrentSpeed += Acceleration * delta;

                if (CurrentSpeed > MaxSpeed) CurrentSpeed = MaxSpeed;

                float animationSpeed = 1 + 2 * (CurrentSpeed / MaxSpeed);

                ATree.Set("parameters/TimeScale/scale", animationSpeed);

                return Velocity.MoveToward(inputVelocity * CurrentSpeed, Acceleration * delta);
            }

            ATreePlayback.Travel("Idle");
            ATree.Set("parameter/playback", "Idle");

            CurrentSpeed = 0;

            return Velocity.MoveToward(Vector2.Zero, Friction * delta);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Input.IsActionPressed("up")) PlayerDirection = Up;

            if (Input.IsActionPressed("down")) PlayerDirection = Down;

            if (Input.IsActionPressed("left")) PlayerDirection = Left;

            if (Input.IsActionPressed("right")) PlayerDirection = Right;

            Velocity = MoveAndSlide(GetInput(delta));

            if (IsMoving)
            {
            }

            if (!Sword.Swinging)
                switch (PlayerDirection)
                {
                    case Up:
                        Sword.Rotation = 0f;
                        Sword.Position = new Vector2(0, -15);
                        break;
                    case Down:
                        Sword.Rotation = Convert.ToSingle(Math.PI);
                        Sword.Position = new Vector2(0, 15);
                        break;
                    case Left:
                        Sword.Rotation = Convert.ToSingle(3 * Math.PI / 2);
                        Sword.Position = new Vector2(-16, -5);
                        break;
                    case Right:
                        Sword.Rotation = Convert.ToSingle(Math.PI / 2d);
                        Sword.Position = new Vector2(16, -5);
                        break;
                }

            if ((Position - lastPosition).Abs().LengthSquared() > 256)
            {
                EmitSignal("moved", Position.x, Position.y);
                lastPosition = Position;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("slash")) Sword.Swing();
        }
    }
}