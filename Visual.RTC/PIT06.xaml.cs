using System;
using System.Windows;
using RTF = RayTracerChallenge.Features;
using RTH = RayTracerChallenge.Helpers;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT06.xaml
    /// </summary>
    public partial class PIT06 : Window
    {
        public PIT06()
        {
            InitializeComponent();
        }

        public RTF.Canvas CreateCircle(RTF.Shapes.Sphere s)
        {
            var lightPos = RTF.PointType.Point(-10, 10, -10);
            var light = new RTF.Light(lightPos, RTF.Color.White());

            var rayOrigin = RTF.PointType.Point(0, 0, -5);
            double wallZ = 10;
            double wallSize = 7;
            var canvasPixel = 100;
            double pixelSize = wallSize / canvasPixel;
            double half = wallSize / 2;

            var canvas = new RTF.Canvas(canvasPixel, canvasPixel, RTF.Color.Black());

            var shape = s;
            shape.Material.Color = RTF.Color.Magenta();

            for (int y = 0; y < canvasPixel; y++)
            {
                var worldY = half - pixelSize * y;
                for (int x = 0; x < canvasPixel; x++)
                {
                    var worldX = -half + pixelSize * x;
                    var position = RTF.PointType.Point(worldX, worldY, wallZ);

                    var r = new RTF.Ray(rayOrigin, (position - rayOrigin).Normalize());
                    var xs = RTF.Intersection.Intersect(shape, r);

                    var hit = RTF.Intersection.Hit(xs);

                    if (hit != null)
                    {
                        var point = RTH.Transformations.Position(r, hit.T);
                        var normal = RTF.Light.NormalAt(hit.Object, point);
                        var eye = -r.Direction;
                        
                        var color = RTF.Light.Lighting(
                            (hit.Object as RTF.Shapes.Sphere).Material,
                            light,
                            point,
                            eye,
                            normal,
                            false);

                        canvas.WritePixel(x, y, color);
                    }
                }
            }

            return canvas;
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
            var canvas = CreateCircle(new RTF.Shapes.Sphere());

            canvas.SaveAsPPMFile(FolderPath.Text + "\\Light.ppm");
        }

        private void ShearingScalingButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new RTF.Shapes.Sphere
            {
                Transform = RTH.Transformations.Scaling(0.5, 1, 1) * RTH.Transformations.Shearing(1, 0, 0, 0, 0, 0)
            };
            var canvas = CreateCircle(s);

            canvas.SaveAsPPMFile(FolderPath.Text + "\\Light[Shearing Scaling].ppm");
        }

        private void RotationScalingButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new RTF.Shapes.Sphere
            {
                Transform = RTH.Transformations.Scaling(0.5, 1, 1) * RTH.Transformations.RotationZ(Math.PI / 4)
            };
            var canvas = CreateCircle(s);

            canvas.SaveAsPPMFile(FolderPath.Text + "\\Light[Rotation Scaling].ppm");
        }

        private void ScalingXButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new RTF.Shapes.Sphere
            {
                Transform = RTH.Transformations.Scaling(0.5, 1, 1)
            };
            var canvas = CreateCircle(s);

            canvas.SaveAsPPMFile(FolderPath.Text + "\\Light[Scaling X].ppm");
        }

        private void ScalingYButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new RTF.Shapes.Sphere
            {
                Transform = RTH.Transformations.Scaling(1, 0.5, 1)
            };
            var canvas = CreateCircle(s);

            canvas.SaveAsPPMFile(FolderPath.Text + "\\Light[Scaling Y].ppm");
        }
    }
}
