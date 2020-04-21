using System;
using System.Windows;
using obj = Visual.RTC.Objects;
using RTCf = RayTracerChallenge.Features;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT02.xaml
    /// </summary>
    public partial class PIT02 : Window
    {
        readonly RTCf.Color black = new RTCf.Color(0, 0, 0);
        readonly RTCf.Color red = new RTCf.Color(1, 0, 0);

        public PIT02()
        {
            InitializeComponent();
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
            GenerateButton.IsEnabled = false;
            var canvasSize = new Tuple<int, int>(int.Parse(WidthCanvas.Text), int.Parse(HeightCanvas.Text));
            RTCf.Canvas canvas = GenerateProjectile(canvasSize.Item1, canvasSize.Item2);


            string filename = $"{FolderPath.Text}\\{FileName.Text}[{WidthCanvas.Text}x{HeightCanvas.Text}][{VelocityX.Text},{VelocityY.Text},{VelocityZ.Text}][{Magnetude.Text}].ppm";
            canvas.SaveAsPPMFile(filename);
            GenerateButton.IsEnabled = true;
        }

        private RTCf.Canvas GenerateProjectile(int width, int height)
        {
            var canvas = new RTCf.Canvas(width, height, black);

            var m = double.Parse(Magnetude.Text);
            var vel = new Tuple<double, double, double>(
                double.Parse(VelocityX.Text),
                double.Parse(VelocityY.Text),
                double.Parse(VelocityZ.Text));

            var proj = new obj.Projectile(
                    RTCf.PointType.Point(0, 1, 0),
                    RTCf.PointType.Vector(vel.Item1, vel.Item2, vel.Item3).Normalize() * m);
            var env = new obj.Environment(
                    RTCf.PointType.Vector(0, -0.1, 0),
                    RTCf.PointType.Vector(-0.01, 0, 0));

            canvas.WritePixel((int)proj.Position.X, (height - 1) - (int)proj.Position.Y, red);

            foreach (var coord in obj.Projectile.GetTick(env, proj))
            {
                canvas.WritePixel((int)coord.X, (height - 1) - (int)coord.Y, red);
            }

            return canvas;
        }
    }
}
