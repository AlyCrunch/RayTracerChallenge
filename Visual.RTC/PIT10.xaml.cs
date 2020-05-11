using System;
using System.Collections.Generic;
using System.Windows;
using point = RayTracerChallenge.Features.PointType;
using RTF = RayTracerChallenge.Features;
using transform = RayTracerChallenge.Helpers.Transformations;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT10.xaml
    /// </summary>
    public partial class PIT10 : Window
    {
        public PIT10()
        {
            InitializeComponent();
        }
        public RTF.Canvas CreateCeiling(RTF.Shapes.Shape ceilling)
        {
            var world = new RTF.World
            {
                Light = new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White),
                Objects = new List<RTF.Shapes.Shape>() { ceilling }
            };

            var camera = new RTF.Camera(400, 200, Math.PI / 3)
            {
                Transform = transform.ViewTransform(
                    point.Point(0, 1.5, -5),
                    point.Point(0, 1, 0),
                    point.Vector(0, 1, 0))
            };

            return RTF.Canvas.Render(camera, world);
        }

        public RTF.Canvas CreatingPerturbedSphere()
        {
            var p_ceiling = new RTF.Patterns.Checker(RTF.Color.Black, RTF.Color.White);

            var ceilling = new RTF.Shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Pattern = new RTF.Patterns.Perturbed(p_ceiling),
                    Color = RTF.Color.Black,
                    Specular = 0
                }
            };

            var p_sphere = new RTF.Patterns.Ring(RTF.Color.Purple, RTF.Color.Pink);
            var middle = new RTF.Shapes.Sphere()
            {
                Transform = transform.Translation(-0.5, 1, 0.5),
                Material = new RTF.Material()
                {
                    Pattern = new RTF.Patterns.Perturbed(p_sphere),
                    Color = RTF.Color.Black,
                    Specular = 0

                }
            };

            var world = new RTF.World
            {
                Light = new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White),
                Objects = new List<RTF.Shapes.Shape>() { ceilling, middle }
            };

            var camera = new RTF.Camera(400, 200, Math.PI / 3)
            {
                Transform = transform.ViewTransform(
                    point.Point(0, 1.5, -5),
                    point.Point(0, 1, 0),
                    point.Vector(0, 1, 0))
            };

            return RTF.Canvas.Render(camera, world);
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();

                FolderPath.Text = dialog.SelectedPath;
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            var ceilling = new RTF.Shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Pattern = new RTF.Patterns.RadialGradient(RTF.Color.Blue, RTF.Color.White),
                    Color = RTF.Color.Black,
                    Specular = 0
                }
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreateCeiling(ceilling);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\GradientRadiant[{elapsedMs}ms].ppm");
        }

        private void NestedButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new RTF.Patterns.Checker(RTF.Color.Black, RTF.Color.White);
            var a = new RTF.Patterns.Stripe(RTF.Color.Black, RTF.Color.Gray);
            var b = new RTF.Patterns.Stripe(RTF.Color.Red, RTF.Color.Purple)
            {
                Transform = transform.RotationY(Math.PI / 2)
            };

            var ceilling = new RTF.Shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Pattern = new RTF.Patterns.Nested(main, a, b),
                    Color = RTF.Color.Black,
                    Specular = 0
                }
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreateCeiling(ceilling);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\NestedPattern[{elapsedMs}ms].ppm");
        }

        private void BlendedButton_Click(object sender, RoutedEventArgs e)
        {
            var a = new RTF.Patterns.Stripe(RTF.Color.Green, RTF.Color.White);
            var b = new RTF.Patterns.Stripe(RTF.Color.Green, RTF.Color.White)
            {
                Transform = transform.RotationY(Math.PI / 2)
            };

            var ceilling = new RTF.Shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Pattern = new RTF.Patterns.Blended(a,b),
                    Color = RTF.Color.Black,
                    Specular = 0
                }
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreateCeiling(ceilling);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\BlendedPattern[{elapsedMs}ms].ppm");
        }

        private void PerturbatedButton_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreatingPerturbedSphere();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\PerturbatedPattern[{elapsedMs}ms].ppm");
        }
    }
}
