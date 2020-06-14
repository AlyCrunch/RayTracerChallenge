using System;
using System.Collections.Generic;
using System.Windows;
using RTF = RayTracerChallenge.Features;
using point = RayTracerChallenge.Features.PointType;
using transform = RayTracerChallenge.Helpers.Transformations;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT11.xaml
    /// </summary>
    public partial class PIT11 : Window
    {
        public PIT11()
        {
            InitializeComponent();
        }

        public RTF.Canvas CreateWorld()
        {
            var ceilling = new RTF.Shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Color = new RTF.Color(1, 0.9, 0.9),
                    Specular = 0
                }
            };

            var middle = new RTF.Shapes.Sphere()
            {
                Transform = transform.Translation(-0.5, 1, 0.5),
                Material = new RTF.Material()
                {
                    Color = RTF.Color.Green,
                    Diffuse = 0.1,
                    Specular = 1,
                    Transparency = 1,
                    Shininess = 300,
                    Reflective = 0.9
                }
            };

            var world = new RTF.World
            {
                Lights = new List<RTF.Light>() { new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White) },
                Objects = new List<RTF.Shapes.Shape>() { ceilling, middle }
            };

            var camera = new RTF.Camera(200, 100, Math.PI / 3)
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
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreateWorld();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\ReflectionRefraction[{elapsedMs}ms].ppm");
        }
    }
}
