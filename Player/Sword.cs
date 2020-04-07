using Godot;
using System;

public class Sword : KinematicBody2D
{
    private AnimationPlayer CollisionAnimator;
    private AnimationPlayer SpriteAnimator;

    private bool ACollisionPlaying = false;
    private bool ASpritePlaying = false;

    public bool Swinging
    {
        get
        {
            return (ACollisionPlaying || ASpritePlaying);
        }
    }

    public override void _Ready()
    {
        Visible = false;
        CollisionAnimator = GetNode<AnimationPlayer>("CollisionAnimator");
        SpriteAnimator = GetNode<AnimationPlayer>("SpriteAnimator");

        CollisionAnimator.Connect("animation_finished", this, nameof(ACollisionFinished));
        SpriteAnimator.Connect("animation_finished", this, nameof(ASpriteFinished));
    }

    public void ACollisionFinished(String animName)
    {
        ACollisionPlaying = false;
        CollisionAnimator.Seek(0);
    }

    public void ASpriteFinished(String animName)
    {
        ASpritePlaying = false;
        SpriteAnimator.Seek(0);

        if (Visible)
        {
            Visible = false;
        }
    }

    public void Swing()
    {
        if (!(ACollisionPlaying || ASpritePlaying))
        {
            Visible = true;
            ACollisionPlaying = true;
            ASpritePlaying = true;

            CollisionAnimator.Play("SwordCollisionShapeMover");
            SpriteAnimator.Play("slash");
        }
    }
}