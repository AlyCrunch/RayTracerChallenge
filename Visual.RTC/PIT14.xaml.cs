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
    /// Logique d'interaction pour PIT14.xaml
    /// </summary>
    public partial class PIT14 : Window
    {
        public PIT14()
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
            var hex = Hexagon();
            hex.Transform = transform.Translation(0, 1, 0) * transform.RotationX(Math.PI / 2);
            hex.Material = new RTF.Material()
            {
                Color = RTF.Color.White
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreateWorld(new List<shapes.Shape>() { hex });
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\HexagonalRing[{elapsedMs}ms].ppm");
        }

        private shapes.Sphere HexagonCorner()
            => new shapes.Sphere(transform.Translation(0, 0, -1) * transform.Scaling(0.25, 0.25, 0.25));
        private shapes.Cylinder HexagonEdge()
            => new shapes.Cylinder()
            {
                Minimum = 0,
                Maximum = 1,
                Transform = transform.Translation(0, 0, -1)
                            * transform.RotationY(-Math.PI / 6)
                            * transform.RotationZ(-Math.PI / 2)
                            * transform.Scaling(0.25, 1, 0.25)
            };
        private shapes.Group HexagonSide()
            => new shapes.Group(new List<shapes.Shape>() { HexagonCorner(), HexagonEdge() });
        private shapes.Group Hexagon()
        {
            var hex = new shapes.Group();
            for (int i = 0; i <= 5; i++)
            {
                var side = HexagonSide();
                side.Transform = transform.RotationY(i * Math.PI / 3);
                hex.Add(side);
            }
            return hex;
        }
    }
}
