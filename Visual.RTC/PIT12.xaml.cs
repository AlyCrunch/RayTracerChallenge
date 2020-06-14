using System;
using System.Collections.Generic;
using System.Windows;
using RTF = RayTracerChallenge.Features;
using point = RayTracerChallenge.Features.PointType;
using transform = RayTracerChallenge.Helpers.Transformations;
using shapes = RayTracerChallenge.Features.Shapes;
using patterns = RayTracerChallenge.Features.Patterns;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT12.xaml
    /// </summary>
    public partial class PIT12 : Window
    {
        public PIT12()
        {
            InitializeComponent();
        }

        public RTF.Canvas CreateWorld(List<shapes.Shape> shapes)
        {
            var ceilling = new shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Color = new RTF.Color(1, 0.9, 0.9),
                    Specular = 0
                }
            };

            shapes.Add(ceilling);

            var world = new RTF.World
            {
                Lights = new List<RTF.Light>() { new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White) },
                Objects = shapes
            };

            var camera = new RTF.Camera(200, 100, Math.PI / 3)
            {
                Transform = transform.ViewTransform(
                    point.Point(0, 6, -20),
                    point.Point(0, 3, 0),
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

        private void IceCreamButton_Click(object sender, RoutedEventArgs e)
        {
            List<shapes.Shape> shapes = new List<shapes.Shape>();

            var a = new patterns.Stripe(RTF.Color.Maroon, RTF.Color.White);

            var cone = new shapes.Cone()
            {
                Transform = transform.Scaling(0.5, 1, 0.5),
                Material = new RTF.Material()
                {
                    Pattern = a,
                    Specular = 0
                },
                Maximum = 3,
                Minimum = 0
            };
            shapes.Add(cone);

            var strawberry = new shapes.Sphere()
            {
                Transform = transform.Scaling(2, 2, 2) * transform.Translation(0, 2, 0),
                Material = new RTF.Material()
                {
                    Pattern = new patterns.Perturbed(new patterns.Stripe(RTF.Color.Pink, RTF.Color.Red)),
                    Reflective = 0.8,
                    Shininess = 200
                }
            };

            shapes.Add(strawberry);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreateWorld(shapes);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\IceCream[{elapsedMs}ms].ppm");

        }
    }
}
