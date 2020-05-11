using System;
using System.Collections.Generic;
using System.Windows;
using point = RayTracerChallenge.Features.PointType;
using RTF = RayTracerChallenge.Features;
using transform = RayTracerChallenge.Helpers.Transformations;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT07.xaml
    /// </summary>
    public partial class PIT09 : Window
    {
        public PIT09()
        {
            InitializeComponent();
        }

        public RTF.Canvas CreateCircle()
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
                Material = new RTF.Material(RTF.Color.Purple, 0.7, 0.3)
            };
            var right = new RTF.Shapes.Sphere()
            {
                Transform = transform.Translation(1.5, 0.5, -0.5) * transform.Scaling(0.5, 0.5, 0.5),
                Material = new RTF.Material(RTF.Color.Lime, 0.7, 0.3)
            };
            var left = new RTF.Shapes.Sphere()
            {
                Transform = transform.Translation(-1.5, 0.33, -0.75) * transform.Scaling(0.33, 0.33, 0.33),
                Material = new RTF.Material(RTF.Color.Cyan, 0.7, 0.3)
            };

            var world = new RTF.World
            {
                Light = new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White),
                Objects = new List<RTF.Shapes.Shape>() { ceilling, right, middle, right, left }
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
            var canvas = CreateCircle();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\Plane[{elapsedMs}ms].ppm");
        }

    }
}