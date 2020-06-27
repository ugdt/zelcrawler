using Godot;

namespace zelcrawler.player
{
    public class PlayerCamera : Camera2D
    {
        private Vector2 _step, _min, _max = Vector2.Zero;
        [Export] private float Max = 15f;
        [Export] private float Min = .5f;
        [Export] private float Step = 0.5f;
        
        private int _zoomSpeed = 5;
        private Vector2 _cachedZoom = Vector2.Zero;

        public override void _Ready()
        {
            _step = new Vector2(Step, Step);
            _min = new Vector2(Min, Min);
            _max = new Vector2(Max, Max);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_cachedZoom != Vector2.Zero)
            {
                Vector2 zoomEnding = Zoom + _cachedZoom;

                if (zoomEnding < _min)
                {
                    zoomEnding = _min;
                }
                else if (zoomEnding > _max)
                {
                    zoomEnding = _max;
                }
                
                Zoom = Zoom.LinearInterpolate(zoomEnding, _zoomSpeed * delta);

                _cachedZoom = Vector2.Zero;
            }
        }

        public override void _Input(InputEvent e)
        {
            if (e.IsActionReleased("zoom_in"))
            {
                _cachedZoom -= _step * 2;
            }

            if (e.IsActionReleased("zoom_out"))
            {
                _cachedZoom += _step;
            }

            if (e.IsActionPressed("zoom_reset")) Zoom = Vector2.One;
        }
    }
}