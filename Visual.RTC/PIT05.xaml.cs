using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RTF = RayTracerChallenge.Features;
using RTH = RayTracerChallenge.Helpers;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT05.xaml
    /// </summary>
    public partial class PIT05 : Window
    {
        public PIT05()
        {
            InitializeComponent();
        }

        public RTF.Canvas DrawRedCircle(RTF.Sphere s)
        {
            var rayOrigin = RTF.PointType.Point(0, 0, -5);
            double wallZ = 10;
            double wallSize = 7;
            var canvasPixel = 100;
            double pixelSize = (double)wallSize / (double)canvasPixel;
            double half = wallSize / 2;

            var canvas = new RTF.Canvas(canvasPixel, canvasPixel, new RTF.Color(0, 0, 0));
            var color = new RTF.Color(255, 0, 0);
            var shape = s;

            for (int y = 0; y < canvasPixel; y++)
            {
                var worldY = half - pixelSize * y;
                for (int x = 0; x < canvasPixel; x++)
                {
                    var worldX = -half + pixelSize * x;
                    var position = RTF.PointType.Point(worldX, worldY, wallZ);

                    var r = new RTF.Ray(rayOrigin, (position - rayOrigin).Normalizing());
                    var xs = RTF.Intersection.Intersect(shape, r);

                    var hit = RTF.Intersection.Hit(xs);

                    if (hit != null)
                        canvas.WritePixel(x, y, color);
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
            var canvas = DrawRedCircle(new RTF.Sphere());

            canvas.SaveAsPPMFile(FolderPath.Text + "\\RaySphereIntersections.ppm");
        }

        private void ShearingScalingButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new RTF.Sphere
            {
                Transform = RTH.Transformations.Scaling(0.5, 1, 1) * RTH.Transformations.Shearing(1, 0, 0, 0, 0, 0)
            };
            var canvas = DrawRedCircle(s);

            canvas.SaveAsPPMFile(FolderPath.Text + "\\RaySphereIntersections[Shearing Scaling].ppm");
        }

        private void RotationScalingButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new RTF.Sphere
            {
                Transform = RTH.Transformations.Scaling(0.5, 1, 1) * RTH.Transformations.RotationZ(Math.PI / 4)
            };
            var canvas = DrawRedCircle(s);

            canvas.SaveAsPPMFile(FolderPath.Text + "\\RaySphereIntersections[Rotation Scaling].ppm");
        }

        private void ScalingXButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new RTF.Sphere
            {
                Transform = RTH.Transformations.Scaling(0.5, 1, 1)
            };
            var canvas = DrawRedCircle(s);

            canvas.SaveAsPPMFile(FolderPath.Text + "\\RaySphereIntersections[Scaling X].ppm");
        }

        private void ScalingYButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new RTF.Sphere
            {
                Transform = RTH.Transformations.Scaling(1, 0.5, 1)
            };
            var canvas = DrawRedCircle(s);

            canvas.SaveAsPPMFile(FolderPath.Text + "\\RaySphereIntersections[Scaling Y].ppm");
        }
    }
}
