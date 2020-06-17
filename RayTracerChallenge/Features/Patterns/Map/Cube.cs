using System;
using System.Collections.Generic;

namespace RayTracerChallenge.Features.Patterns.Map
{
    public class Cube : RayTracerChallenge.Features.Patterns.Pattern
    {
        Dictionary<Faces, Pattern> Faces { get; set; } = new Dictionary<Faces, Pattern>();

        public Cube(Pattern left, Pattern front, Pattern right, Pattern back, Pattern up, Pattern down)
        {
            Faces.Add(Map.Faces.Left, left);
            Faces.Add(Map.Faces.Front, front);
            Faces.Add(Map.Faces.Right, right);
            Faces.Add(Map.Faces.Back, back);
            Faces.Add(Map.Faces.Up, up);
            Faces.Add(Map.Faces.Down, down);
        }

        public override Color At(PointType p)
        {
            var face = FaceFromPoint(p);

            if (face == Map.Faces.Left)
                return Faces[Map.Faces.Left].At(GetUVLeft(p));
            if (face == Map.Faces.Right)
                return Faces[Map.Faces.Right].At(GetUVRight(p));
            if (face == Map.Faces.Front)
                return Faces[Map.Faces.Front].At(GetUVFront(p));
            if (face == Map.Faces.Back)
                return Faces[Map.Faces.Back].At(GetUVBack(p));
            if (face == Map.Faces.Up)
                return Faces[Map.Faces.Up].At(GetUVUp(p));
            if (face == Map.Faces.Down)
                return Faces[Map.Faces.Down].At(GetUVDown(p));

            throw new Exception("No face found.");
        }

        static public Faces FaceFromPoint(PointType point)
        {
            var absX = Math.Abs(point.X);
            var absY = Math.Abs(point.Y);
            var absZ = Math.Abs(point.Z);
            var coord = Math.Max(absX, Math.Max(absY, absZ));

            if (coord == point.X) return Map.Faces.Right;
            if (coord == -point.X) return Map.Faces.Left;
            if (coord == point.Y) return Map.Faces.Up;
            if (coord == -point.Y) return Map.Faces.Down;
            if (coord == point.Z) return Map.Faces.Front;
            if (coord == -point.Z) return Map.Faces.Back;

            throw new Exception("No faces found");
        }

        public static (double u, double v) GetUVFront(PointType p)
        {
            var u = ((p.X + 1) % 2.0) / 2.0;
            var v = ((p.Y + 1) % 2.0) / 2.0;

            return (u, v);
        }

        public static (double u, double v) GetUVBack(PointType p)
        {
            var u = ((1 - p.X) % 2.0) / 2.0;
            var v = ((p.Y + 1) % 2.0) / 2.0;

            return (u, v);
        }

        public static (double u, double v) GetUVLeft(PointType p)
        {
            var u = ((p.Z + 1) % 2.0) / 2.0;
            var v = ((p.Y + 1) % 2.0) / 2.0;

            return (u, v);
        }

        public static (double u, double v) GetUVRight(PointType p)
        {
            var u = ((1 - p.Z) % 2.0) / 2.0;
            var v = ((p.Y + 1) % 2.0) / 2.0;

            return (u, v);
        }

        public static (double u, double v) GetUVUp(PointType p)
        {
            var u = ((p.X + 1) % 2.0) / 2.0;
            var v = ((1 - p.Z) % 2.0) / 2.0;

            return (u, v);
        }

        public static (double u, double v) GetUVDown(PointType p)
        {
            var u = ((p.X + 1) % 2.0) / 2.0;
            var v = ((p.Z + 1) % 2.0) / 2.0;

            return (u, v);
        }
    }

    public enum Faces
    {
        Left,
        Right,
        Front,
        Back,
        Up,
        Down
    }
}
