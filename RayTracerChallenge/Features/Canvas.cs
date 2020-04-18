using System;
using System.Collections.Generic;
using System.IO;

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
                    Pixels[x, y] = color ?? new Color(0, 0, 0);
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

        public void SaveAsPPMFile(string path)
        {
            File.WriteAllLines(path, CreatePPMHeader());
            File.AppendAllLines(path, CreatePPMCanvas());
        }

        public static Canvas Render(Camera camera, World world)
        {
            var image = new Canvas(camera.HorizontalSize, camera.VerticalSize);

            for (int y = 0; y < camera.VerticalSize; y++)
            {
                for (int x = 0; x < camera.HorizontalSize; x++)
                {
                    var ray = camera.RayForPixel(x, y);
                    var color = world.ColorAt(ray);
                    image.WritePixel(x, y, color);
                }
            }

            return image;
        }
    }
}