using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using RTF = RayTracerChallenge.Features;
using FP = RayTracerChallenge.Helpers.FileParser;
using point = RayTracerChallenge.Features.PointType;
using transform = RayTracerChallenge.Helpers.Transformations;
using shapes = RayTracerChallenge.Features.Shapes;
using patterns = RayTracerChallenge.Features.Patterns;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT16.xaml
    /// </summary>
    public partial class PIT16 : Window
    {
        public PIT16()
        {
            InitializeComponent();
        }

        public RTF.Canvas CreateWorld(List<shapes.Shape> shapes)
        {
            //var ceilling = new shapes.Plane()
            //{
            //    Material = new RTF.Material()
            //    {
            //        Color = new RTF.Color(1, 0.9, 0.9),
            //        Specular = 0
            //    }
            //};

            //shapes.Add(ceilling);

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

        private void TeaPotLR_Click(object sender, RoutedEventArgs e)
        {
            var parser = new FP();
            parser.Parse(@".\Files\Teapot (low res).obj");
            var teapot = parser.ObjToGroup();
            teapot.Transform = transform.RotationZ(Math.PI / 2);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreateWorld(new List<shapes.Shape>() { teapot });
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\Teapot_LR[{elapsedMs}ms].ppm");
        }

        private void Lens_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BlockLetter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Flower_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Saturn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
