using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Client.View
{
    public class Camera2D
    {
        private float _zoom; // Camera Zoom
        private readonly float _rotation; // Camera Rotation

        public Vector2 Position { get; set; } // Camera Position

        private Matrix _transform; // Matrix Transform

        private readonly int _width;
        private readonly int _height;

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public Camera2D(int width, int height)
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
         
            Position = Vector2.Zero;

            _width = width;
            _height = height;

        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {

            float viewWidthOver2 = graphicsDevice.Viewport.Width * 0.5f;
            float viewHeightOver2 = graphicsDevice.Viewport.Height * 0.5f;

            //check x axis
            float transformX = Position.X - viewWidthOver2;
            transformX = MathHelper.Clamp(transformX, 0, _width - 2 * viewWidthOver2);
           
            //check y axis
            float transformY = Position.Y - viewHeightOver2;
            transformY = MathHelper.Clamp(transformY, 0, _height - 2 * viewHeightOver2);

            _transform = Matrix.CreateTranslation(new Vector3(-transformX, -transformY, 0)) *
                         Matrix.CreateRotationZ(_rotation) *
                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));

            return _transform;
        }

        public Vector2 ConvertToLocal(Vector2 coordinates)
        {
            return new Vector2(_transform.Translation.X + coordinates.X, 
                _transform.Translation.Y + coordinates.Y);
        }

    }
}
