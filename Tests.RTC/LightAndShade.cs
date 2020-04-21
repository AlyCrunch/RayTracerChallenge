using System;
using RTF = RayTracerChallenge.Features;
using RTH = RayTracerChallenge.Helpers;
using pt = RayTracerChallenge.Features.PointType;
using RayTracerChallenge.Features.Shapes;
using Tests.RTC.Helpers;
using Xunit;

namespace Tests.RTC
{
    public class LightAndShade
    {
        [Fact]
        public void NormalSpherePointX()
        {
            var s = new Sphere();
            pt n = s.NormalAt(pt.Point(1, 0, 0));
            var e = pt.Vector(1, 0, 0);
            Assert.Equal(e, n);
        }
        [Fact]
        public void NormalSpherePointY()
        {
            var s = new Sphere();
            pt n = s.NormalAt(pt.Point(0, 1, 0));
            var e = pt.Vector(0, 1, 0);
            Assert.Equal(e, n);
        }
        [Fact]
        public void NormalSpherePointZ()
        {
            var s = new Sphere();
            pt n = s.NormalAt(pt.Point(0, 0, 1));
            var e = pt.Vector(0, 0, 1);
            Assert.Equal(e, n);
        }
        [Fact]
        public void NormalSphereNonAxialPoint()
        {
            var s = new Sphere();
            double value = Math.Sqrt(3) / 3;
            pt n = s.NormalAt(pt.Point(value, value, value));
            var e = pt.Vector(value, value, value);
            Assert.Equal(e, n);
        }
        [Fact]
        public void NormalIsNormalizedVector()
        {
            var s = new Sphere();
            double value = Math.Sqrt(3) / 3;

            pt n = s.NormalAt(pt.Point(value, value, value));

            Assert.Equal(n, n.Normalize());
        }

        [Fact]
        public void NormalOnTranslatedSphere()
        {
            var s = new Sphere
            {
                Transform = RTH.Transformations.Translation(0, 1, 0)
            };

            pt n = s.NormalAt(pt.Point(0, 1.70711, -0.70711));
            var e = pt.Vector(0, 0.70711, -0.70711);

            CustomAssert.Equal(e, n, 5);
        }
        [Fact]
        public void NormalOnTransformedSphere()
        {
            var s = new Sphere
            {
                Transform = RTH.Transformations.Scaling(1, 0.5, 1) * RTH.Transformations.RotationZ(Math.PI / 5)
            };

            pt n = s.NormalAt(pt.Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));
            var e = pt.Vector(0, 0.97014, -0.24254);

            CustomAssert.Equal(e, n, 5);
        }

        [Fact]
        public void ReflectingVector45deg()
        {
            var v = pt.Vector(1, -1, 0);
            var n = pt.Vector(0, 1, 0);

            pt r = RTF.Light.Reflect(v, n);

            var e = pt.Vector(1, 1, 0);
            Assert.Equal(e, r);
        }
        [Fact]
        public void ReflectingVectorOffSlanted()
        {
            var v = pt.Vector(0, -1, 0);
            var n = pt.Vector(Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0);

            pt r = RTF.Light.Reflect(v, n);

            var e = pt.Vector(1, 0, 0);
            CustomAssert.Equal(e, r, 5);
        }

        [Fact]
        public void PointLightHasPositionAndIntensity()
        {
            var intensity = new RTF.Color(1, 1, 1);
            var position = pt.Point(0, 0, 0);
            var light = new RTF.Light(position, intensity);

            Assert.Equal(position, light.Position);
            Assert.Equal(intensity, light.Intensity);
        }

        [Fact]
        public void DefaultMaterial()
        {
            var m = new RTF.Material();
            Assert.Equal(new RTF.Color(1, 1, 1), m.Color);
            Assert.Equal(0.1, m.Ambient);
            Assert.Equal(0.9, m.Diffuse);
            Assert.Equal(0.9, m.Specular);
            Assert.Equal(200, m.Shininess);
        }
        [Fact]
        public void SphereDefaultMaterial()
        {
            var s = new Sphere();
            var m = s.Material;
            var e = new RTF.Material();
            Assert.Equal(e, m);
        }
        [Fact]
        public void SphereAssignedMaterial()
        {
            var m = new RTF.Material
            {
                Ambient = 1
            };
            var s = new Sphere
            {
                Material = m
            };

            Assert.Equal(1, s.Material.Ambient);
        }

        [Fact]
        public void LightingWithEyeBetweenLightSurface()
        {
            var m = new RTF.Material();
            var position = pt.Point(0, 0, 0);

            var eyeVector = pt.Vector(0, 0, -1);
            var normalVector = pt.Vector(0, 0, -1);

            var light = new RTF.Light
                (pt.Point(0, 0, -10), new RTF.Color(1, 1, 1));
            var result = RTF.Light.Lighting(m, light, position, eyeVector, normalVector, false);

            CustomAssert.Equal(new RTF.Color(1.9, 1.9, 1.9), result, 5);

        }
        [Fact]
        public void LightingWithEyeBetweenLightSurfaceOffset45()
        {
            var m = new RTF.Material();
            var position = pt.Point(0, 0, 0);

            var eyeVector = pt.Vector(0, Math.Sqrt(2)/2, -Math.Sqrt(2) / 2);
            var normalVector = pt.Vector(0, 0, -1);

            var light = new RTF.Light
                (pt.Point(0, 0, -10), new RTF.Color(1, 1, 1));
            var result = RTF.Light.Lighting(m, light, position, eyeVector, normalVector, false);
            
            Assert.Equal(new RTF.Color(1, 1, 1), result);

        }
        [Fact]
        public void LightingWithEyeOppositeSurfaceOffset45()
        {
            var m = new RTF.Material();
            var position = pt.Point(0, 0, 0);

            var eyeVector = pt.Vector(0, 0, -1);
            var normalVector = pt.Vector(0, 0, -1);

            var light = new RTF.Light
                (pt.Point(0, 10, -10), new RTF.Color(1, 1, 1));
            var result = RTF.Light.Lighting(m, light, position, eyeVector, normalVector, false);

            CustomAssert.Equal(new RTF.Color(0.7364, 0.7364, 0.7364), result, 4);
        }
        [Fact]
        public void LightingWithEyePathReflection()
        {
            var m = new RTF.Material();
            var position = pt.Point(0, 0, 0);

            var eyeVector = pt.Vector(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            var normalVector = pt.Vector(0, 0, -1);

            var light = new RTF.Light
                (pt.Point(0, 10, -10), new RTF.Color(1, 1, 1));
            var result = RTF.Light.Lighting(m, light, position, eyeVector, normalVector, false);

            CustomAssert.Equal(new RTF.Color(1.6364, 1.6364, 1.6364), result, 4);
        }
        [Fact]
        public void LightingWithEyeBehindSurface()
        {
            var m = new RTF.Material();
            var position = pt.Point(0, 0, 0);

            var eyeVector = pt.Vector(0, 0, -1);
            var normalVector = pt.Vector(0, 0, -1);

            var light = new RTF.Light
                (pt.Point(0, 0, 10), new RTF.Color(1, 1, 1));
            var result = RTF.Light.Lighting(m, light, position, eyeVector, normalVector, false);

            Assert.Equal(new RTF.Color(0.1, 0.1, 0.1), result);
        }
    }
}
