using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracerChallenge.Features.Shapes
{
    public class Group : Shape
    {
        public List<Shape> Children { get; set; } = new List<Shape>();

        public Shape this[int index]
        {
            get => Children[index];
            set { Children[index] = value; }
        }

        public Group()
        {
            Transform = Matrix.GetIdentity();
        }
        public Group(IEnumerable<Shape> list)
        {
            Transform = Matrix.GetIdentity();
            Add(list);
        }
        public Group(Matrix m)
        {
            Transform = m;
        }

        #region List manipulation methods
        public void Add(Shape s)
        {
            s.Parent = this;
            Children.Add(s);
        }
        public void Add(IEnumerable<Shape> s)
         => Children.AddRange(s.Select(shape => { shape.Parent = this; return shape; }));

        public void Remove(Shape s)
        {
            s.Parent = null;
            Children.Remove(s);
        }

        public void RemoveAt(int index)
        {
            Children[index].Parent = null;
            Children.RemoveAt(index);
        }

        public bool IsEmpty() => Children.Count == 0;
        public int Count() => Children.Count;
        #endregion

        protected override Intersection[] LocalIntersect(Ray localRay)
        {
            if (!Bounds().Intersects(localRay)) return new Intersection[] { };

            List<Intersection> childrenIntersections = new List<Intersection>();
            foreach (var child in Children)
            {
                childrenIntersections.AddRange(child.Intersect(localRay));
            }

            return childrenIntersections.OrderBy(i => i.T).ToArray();
        }

        protected override PointType LocalNormalAt(PointType localPoint)
        {
            throw new NotImplementedException();
        }

        public override BoundingBox Bounds()
        {
            var bb = BoundingBox.Empty;

            foreach (var child in Children)
                bb += child.ParentSpaceBounds();

            return bb;
        }

        public (List<Shape> left, List<Shape> right) Partition()
        {
            var left = new List<Shape>();
            var right = new List<Shape>();

            (var leftBox, var rightBox) = Bounds().Split();

            var childrenArr = Children.ToArray();
            foreach (var s in childrenArr)
            {
                var sBBox = s.ParentSpaceBounds();

                if (leftBox.ContainsBox(sBBox))
                {
                    Children.Remove(s);
                    left.Add(s);
                    continue;
                }
                if (rightBox.ContainsBox(sBBox))
                {
                    Children.Remove(s);
                    right.Add(s);
                    continue;
                }
            }
            return (left, right);
        }

        public void Subgroup(List<Shape> list)
        {
            var subgroup = new Group
            {
                Parent = this
            };
            subgroup.Add(list.Select(shape => { shape.Parent = subgroup; return shape; }));
            Add(subgroup);
        }

        public void Divide(int threshold)
        {
            if (threshold <= Count())
            {
                (var left, var right) = Partition();
                if (left.Count > 0) Subgroup(left);
                if (right.Count > 0) Subgroup(right);
            }

            foreach (var child in Children)
            {
                if (child is Group)
                    (child as Group).Divide(threshold);
            }
        }
    }
}