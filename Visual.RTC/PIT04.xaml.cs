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
    /// Logique d'interaction pour PIT04.xaml
    /// </summary>
    public partial class PIT04 : Window
    {
        public PIT04()
        {
            InitializeComponent();
        }

        // Y = axis / Z = 12 / x = 3
        public List<RTF.PointType> CreateClock()
        {
            var hour = RTF.PointType.Point(0, 0, 1);

            var clock = new List<RTF.PointType> { hour };

            for (int i = 1; i < 12; i++)
            {
                var rotation = RTH.Transformations.RotationY(i * (Math.PI / 6));
                var newHour = rotation * hour;
                clock.Add(newHour);
            }

            return clock;
        }

        public RTF.Canvas ConvertPointsToCanvas(List<RTF.PointType> points, int width, int height)
        {
            RTF.Canvas c = new RTF.Canvas(width, height, new RTF.Color(0, 0, 0));
            var radius = (3d / 8d) * width;

            var color = new RTF.Color(255, 255, 255);

            foreach(var point in points)
            {
                int x = (int)((radius * point.X) + width / 2);
                int y = (int)((radius * point.Z) + height / 2);

                c.WritePixel(x, y, color);
            }

            return c;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            var clock = CreateClock();

            var width = int.Parse(WidthCanvas.Text);
            var height = int.Parse(HeightCanvas.Text);
            RTF.Canvas canvas = ConvertPointsToCanvas(clock, width, height);

            string filename = $"{FolderPath.Text}\\Clock[{WidthCanvas.Text}x{HeightCanvas.Text}].ppm";
            canvas.SaveAsPPMFile(filename);
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();

                FolderPath.Text = dialog.SelectedPath;
            }
        }
    }
}
