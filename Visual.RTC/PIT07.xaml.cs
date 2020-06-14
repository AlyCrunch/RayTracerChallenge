using System;
using System.Collections.Generic;
using System.Windows;
using point = RayTracerChallenge.Features.PointType;
using RTF = RayTracerChallenge.Features;
using transform = RayTracerChallenge.Helpers.Transformations;
using shapes = RayTracerChallenge.Features.Shapes;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT07.xaml
    /// </summary>
    public partial class PIT07 : Window
    {
        public PIT07()
        {
            InitializeComponent();
        }

        public RTF.Canvas CreateCircle()
        {
            var floor = new shapes.Sphere()
            {
                Transform = transform.Scaling(10, 0.01, 10),
                Material = new RTF.Material()
                {
                    Color = new RTF.Color(1, 0.9, 0.9),
                    Specular = 0
                }
            };

            var leftWall = new shapes.Sphere()
            {
                Transform = transform.Translation(0, 0, 5)
                            * transform.RotationY(-Math.PI / 4)
                            * transform.RotationX(Math.PI / 2)
                            * transform.Scaling(10, 0.01, 10),
                Material = floor.Material
            };

            var rightWall = new shapes.Sphere()
            {
                Transform = transform.Translation(0, 0, 5)
                            * transform.RotationY(Math.PI / 4)
                            * transform.RotationX(Math.PI / 2)
                            * transform.Scaling(10, 0.01, 10),
                Material = floor.Material
            };

            var middle = new shapes.Sphere()
            {
                Transform = transform.Translation(-0.5, 1, 0.5),
                Material = new RTF.Material(new RTF.Color(0.1, 1, 0.5), 0.7, 0.3)
            };

            var right = new shapes.Sphere()
            {
                Transform = transform.Translation(1.5, 0.5, -0.5) * transform.Scaling(0.5, 0.5, 0.5),
                Material = new RTF.Material(new RTF.Color(0.5, 1, 0.1), 0.7, 0.3)
            };

            var left = new shapes.Sphere()
            {
                Transform = transform.Translation(-1.5, 0.33, -0.75) * transform.Scaling(0.33, 0.33, 0.33),
                Material = new RTF.Material(new RTF.Color(1, 0.8, 0.1), 0.7, 0.3)
            };

            var world = new RTF.World
            {
                Lights = new List<RTF.Light>() { new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White) },
                Objects = new List<shapes.Shape>() { floor, leftWall, rightWall, right, middle, right, left }
            };

            var camera = new RTF.Camera(100, 50, Math.PI / 3)
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

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\Scene[{elapsedMs}ms].ppm");
        }

        //private void ShearingScalingButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var s = new shapes.Sphere
        //    {
        //        Transform = RTH.Transformations.Scaling(0.5, 1, 1) * RTH.Transformations.Shearing(1, 0, 0, 0, 0, 0)
        //    };
        //    var canvas = CreateCircle(s);

        //    canvas.SaveAsPPMFile(FolderPath.Text + "\\Light[Shearing Scaling].ppm");
        //}

        //private void RotationScalingButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var s = new shapes.Sphere
        //    {
        //        Transform = RTH.Transformations.Scaling(0.5, 1, 1) * RTH.Transformations.RotationZ(Math.PI / 4)
        //    };
        //    var canvas = CreateCircle(s);

        //    canvas.SaveAsPPMFile(FolderPath.Text + "\\Light[Rotation Scaling].ppm");
        //}

        //private void ScalingXButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var s = new shapes.Sphere
        //    {
        //        Transform = RTH.Transformations.Scaling(0.5, 1, 1)
        //    };
        //    var canvas = CreateCircle(s);

        //    canvas.SaveAsPPMFile(FolderPath.Text + "\\Light[Scaling X].ppm");
        //}

        //private void ScalingYButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var s = new shapes.Sphere
        //    {
        //        Transform = RTH.Transformations.Scaling(1, 0.5, 1)
        //    };
        //    var canvas = CreateCircle(s);

        //    canvas.SaveAsPPMFile(FolderPath.Text + "\\Light[Scaling Y].ppm");
        //}
    }
}
