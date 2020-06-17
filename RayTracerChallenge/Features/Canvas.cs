using RayTracerChallenge.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features
{
    public class Canvas
    {
        #region const
        const string _ID = "P3";
        const int _MAXCOLOR = 255;
        #endregion

        public Color[,] Pixels { get; set; }
        public int Width
        {
            get => Pixels.GetLength(0);
        }
        public int Height
        {
            get => Pixels.GetLength(1);
        }

        public Canvas(int width, int height, Color color = null)
        {
            Pixels = new Color[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    Pixels[x, y] = color ?? Color.Black;
        }

        public void WritePixel(int x, int y, Color color)
        {
            if (x >= Width || x < 0) return;
            if (y >= Height || y < 0) return;

            Pixels[x, y] = color;
        }

        public Color PixelAt(int x, int y) => Pixels[x, y];

        public bool EveryPixelsAre(Color c)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (Pixels[x, y] != c) return false;
            return true;
        }

        public string CanvasToPPM()
            => $"{CreatePPMHeader()} {string.Join("\n", CreatePPMCanvas())}";

        public List<string> CreatePPMHeader()
            => new List<string>()
            {
                $"{_ID}",
                $"{Width} {Height}",
                $"{_MAXCOLOR}"
            };

        public List<string> CreatePPMCanvas()
        {
            List<string> ppm = new List<string>();
            for (int y = 0; y < Height; y++)
            {
                List<string> line = new List<string>();
                for (int x = 0; x < Width; x++)
                    line.Add(Pixels[x, y].To256());

                ppm.Add(string.Join(" ", line));
            }

            return ppm;
        }

        public static Canvas CanvasFromPPM(List<string> file)
        {
            //removing useless lines
            var clearedFile = file.Where(l => l.Length > 0 && l[0] != '#').ToArray();

            //get header
            if (clearedFile[0] != "P3") throw new System.Exception("Format not supported.");
            (int width, int height) = GetSizePPM(clearedFile[1].Split(' '));
            var colorBase = int.Parse(clearedFile[2]);

            var canvas = new Canvas(width, height);

            //queue colors
            var colors = new Queue<string>(clearedFile.Skip(3)
                                    .Select(l => l.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries))
                                    .SelectMany(arr => arr));

            if (colors.Count() % 3 != 0) throw new System.Exception("Invalid format pixel.");
            if (colors.Count() / 3 != width * height) throw new System.Exception("Invalid pixels number.");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var R = int.Parse(colors.Dequeue());
                    var G = int.Parse(colors.Dequeue());
                    var B = int.Parse(colors.Dequeue());
                    canvas.Pixels[x, y] = new Color(R, G, B, colorBase);
                }
            }
            return canvas;
        }

        private static (int width, int height) GetSizePPM(string[] line)
            => (int.Parse(line[0]), int.Parse(line[1]));

        public void SaveAsPPMFile(string path)
        {
            File.WriteAllLines(path, CreatePPMHeader());
            File.AppendAllLines(path, CreatePPMCanvas());
        }

        public static Canvas Render(Camera camera, World world)
        {
            var image = new Canvas(camera.HorizontalSize, camera.VerticalSize);
            int i = 0;
            Parallel.For(0, camera.VerticalSize, y =>
           {
               Parallel.For(0, camera.HorizontalSize, x =>
               {
                   var ray = camera.RayForPixel(x, y);
                   var color = world.ColorAt(ray);
                   image.WritePixel(x, y, color);
                   i++;
                   System.Console.WriteLine($"Pixel processed : {i}");
               });
           });

            return image;
        }
    }
}