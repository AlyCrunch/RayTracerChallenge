using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTH = RayTracerChallenge.Helpers;

namespace RayTracerChallenge.Features
{
    public class Camera
    {
        public int HorizontalSize { get; set; }
        public int VerticalSize { get; set; }
        public double FieldOfView { get; set; }
        public Matrix Transform { get; set; }

        public double Aspect
        {
            get => (double)HorizontalSize / (double)VerticalSize;
        }
        public double HalfView
        {
            get => Math.Tan(FieldOfView / 2);
        }
        public double HalfWidth
        {
            get => (Aspect >= 1) ? HalfView : HalfView * Aspect;
        }
        public double HalfHeight
        {
            get => (Aspect >= 1) ? HalfView / Aspect : HalfView;
        }

        public double PixelSize
        {
            get => (HalfWidth * 2) / (double)HorizontalSize;
        }

        public Camera(int hsize, int vsize, double field_of_view)
        {
            HorizontalSize = hsize;
            VerticalSize = vsize;
            FieldOfView = field_of_view;
            Transform = Matrix.GetIdentity();
        }

        public Ray RayForPixel(int x, int y)
        {
            var xOffset = ((double)x + 0.5) * PixelSize;
            var yOffset = ((double)y + 0.5) * PixelSize;

            var worldX = HalfWidth - xOffset;
            var worldY = HalfHeight - yOffset;

            var pixel = Transform.Inverse() 
                * PointType.Point(worldX, worldY, -1);
            var origin = Transform.Inverse()
                * PointType.Point(0, 0, 0);
            var direction = (pixel - origin).Normalize();

            return new Ray(origin, direction);
        }
    }
}
