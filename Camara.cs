using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AshTechEngine
{
    public class Camera
    {
        public float Zoom;
        public Vector2 Position;
        
        public Rectangle Bounds { get; protected set; }
        
        public Rectangle VisibleArea { get; protected set; }
        public Matrix Transform { get; protected set; }

        public Camera(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            Zoom = 1f;
            Position = Vector2.Zero;

            UpdateMatrix();
        }

        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        public void UpdateCamera(Viewport bounds)
        {
            Bounds = bounds.Bounds;
            UpdateMatrix();
        }

        public Vector2 WorldTScreen(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Transform);
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Matrix.Invert(Transform));
        }
    }


    //// old camara
    ///
    //public class Camera
    //{
    //    public float zoom = 1f;
    //    public float rotationZ = 0;
    //    public int safeZone = 2;
    //    private GameObject follow;
    //    private GraphicsDevice graphicsDevice;

    //    public Vector2 Position;

    //    public Matrix TransformMatrix
    //    {
    //        get
    //        {
    //            return
    //                Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
    //                Matrix.CreateRotationZ(rotationZ) *
    //                Matrix.CreateScale(zoom) *
    //                Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
    //        }
    //    }

    //    public Rectangle Bounds { get; }

    //    public Rectangle VisibleArea
    //    {
    //        get
    //        {
    //            var inverseViewMatrix = Matrix.Invert(TransformMatrix);
    //            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
    //            var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
    //            var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
    //            var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

    //            var min = new Vector2(
    //                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
    //                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
    //            var max = new Vector2(
    //                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
    //                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
    //            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
    //        }
    //    }

    //    public Point RawPositionToCamara(Point position)
    //    {
    //        var screenBounds = graphicsDevice.PresentationParameters.Bounds;
    //        float ratio = Math.Min((float)screenBounds.Width / VisibleArea.Width, (float)screenBounds.Height / VisibleArea.Height);
    //        int renderTargetWidth = (int)(VisibleArea.Width * ratio);
    //        int renderTargetHeight = (int)(VisibleArea.Height * ratio);
    //        int renderTargetPosX = (screenBounds.Width - renderTargetWidth) / 2;
    //        int renderTargetPosY = (screenBounds.Height - renderTargetHeight) / 2;
    //        position.X -= renderTargetPosX;
    //        position.Y -= renderTargetPosY;
    //        position.X = (int)(position.X / ratio) + VisibleArea.X;
    //        position.Y = (int)(position.Y / ratio) + VisibleArea.Y;
    //        return position;
    //    }

    //    public Rectangle VisibleAreaSafe
    //    {
    //        get
    //        {
    //            Rectangle visibleAreaSafe = VisibleArea;
    //            visibleAreaSafe.X = visibleAreaSafe.X - safeZone;
    //            visibleAreaSafe.Y = visibleAreaSafe.Y - safeZone;
    //            visibleAreaSafe.Width = visibleAreaSafe.Width + (safeZone*2);
    //            visibleAreaSafe.Height = visibleAreaSafe.Height + (safeZone*2);

    //            return visibleAreaSafe;
    //        }
    //    }

    //    public Camera(Rectangle bounds, GraphicsDevice graphicsDevice)
    //    { 
    //        Bounds = bounds;
    //        this.graphicsDevice = graphicsDevice;
    //    }

    //    public void FollowGameObject(GameObject follow)
    //    {
    //        this.follow = follow;
    //    }

    //    public Vector2 GetScreenPosition(Vector2 worldPosition)
    //    {
    //        var screenPos = Vector2.Transform(worldPosition, TransformMatrix);

    //        return screenPos;
    //    }

    //    //Cartesian to isometric:
    //    public Vector2 CartesianToIsometric(Vector2 cartesian)
    //    {
    //        Vector2 isometric = new Vector2(0, 0);
    //        isometric.X = cartesian.X - cartesian.Y;
    //        isometric.Y = (cartesian.X + cartesian.Y) / 2;
    //        return isometric;
    //    }
    //    public Vector2 IsometricToCartesian(Vector2 isometric)
    //    {
    //        Vector2 cartesian = new Vector2(0, 0);
    //        cartesian.X = (2 * isometric.Y + isometric.X) / 2;
    //        cartesian.Y = (2 * isometric.Y - isometric.X) / 2;
    //        return cartesian;
    //    }

    //    public void Update(GameTime gameTime)
    //    {
    //        if (follow != null) //Do we have something to follow?
    //        {
    //            Position = new Vector2(follow.Position.X + (follow.Width / 2), follow.Position.Y + (follow.Height / 2));
    //        }
    //    }

    //}
}