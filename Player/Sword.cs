using Godot;

namespace nextGame.Player
{
    public class Sword : KinematicBody2D
    {
        private bool ACollisionPlaying;
        private bool ASpritePlaying;
        private AnimationPlayer CollisionAnimator;
        private AnimationPlayer SpriteAnimator;

        public bool Swinging => ACollisionPlaying || ASpritePlaying;

        public override void _Ready()
        {
            Visible = false;
            CollisionAnimator = GetNode<AnimationPlayer>("CollisionAnimator");
            SpriteAnimator = GetNode<AnimationPlayer>("SpriteAnimator");

            CollisionAnimator.Connect("animation_finished", this, nameof(ACollisionFinished));
            SpriteAnimator.Connect("animation_finished", this, nameof(ASpriteFinished));
        }

        public void ACollisionFinished(string animName)
        {
            ACollisionPlaying = false;
            CollisionAnimator.Seek(0);
        }

        public void ASpriteFinished(string animName)
        {
            ASpritePlaying = false;
            SpriteAnimator.Seek(0);

            if (Visible) Visible = false;
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
}