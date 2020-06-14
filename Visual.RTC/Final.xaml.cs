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
    public partial class Final : Window
    {
        public Final()
        {
            InitializeComponent();
        }

        public RTF.Canvas CreateWorld()
        {
            var camera = new RTF.Camera(100, 100, 0.785)
            {
                Transform = transform.ViewTransform(
                    point.Point(-6, 6, -10),
                    point.Point(6, 0, 6),
                    point.Vector(-0.45, 1, 0))
            };

            var light1 = new RTF.Light(point.Point(50, 100, -50), RTF.Color.White);
            var light2 = new RTF.Light(point.Point(-400, 50, -10), RTF.Color.White);

            var whiteMaterial = new RTF.Material()
            {
                Color = RTF.Color.White,
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0,
                Reflective = 0.1
            };
            var blueMaterial = new RTF.Material()
            {
                Color = new RTF.Color(0.537, 0.831, 0.914),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0,
                Reflective = 0.1
            };
            var redMaterial = new RTF.Material()
            {
                Color = new RTF.Color(0.941, 0.322, 0.388),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0,
                Reflective = 0.1
            };
            var purpleMaterial = new RTF.Material()
            {
                Color = new RTF.Color(0.373, 0.404, 0.550),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0,
                Reflective = 0.1
            };

            var standardTransform = transform.Translation(1, -1, 1) * transform.Scaling(0.5, 0.5, 0.5);
            var largeObject = standardTransform * transform.Scaling(3.5, 3.5, 3.5);
            var mediumObject = standardTransform * transform.Scaling(3, 3, 3);
            var smallObject = standardTransform * transform.Scaling(2, 2, 2);

            var whitebackdrop = new shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Color = RTF.Color.White,
                    Ambient = 1,
                    Diffuse = 0,
                    Specular = 0
                },
                Transform = transform.RotationX(Math.PI / 2) * transform.Translation(0, 0, 500)
            };

            var group = new shapes.Group(
                new List<shapes.Shape>()
                {
                    new shapes.Sphere()
                    {
                        Material = new RTF.Material()
                        {
                            Color = purpleMaterial.Color,
                            Diffuse = 0.2,
                            Ambient = 0,
                            Specular = 1,
                            Shininess = 200,
                            Reflective = 0.7,
                            Transparency = 0.7,
                            RefractiveIndex = 1.5
                        },
                        Transform = largeObject
                    },
                    new shapes.Cube(whiteMaterial, mediumObject * transform.Translation(4, 0, 0)),
                    new shapes.Cube(blueMaterial, largeObject * transform.Translation(8.5, 1.5, -0.5)),
                    new shapes.Cube(redMaterial, largeObject * transform.Translation(0, 0, 4)),
                    new shapes.Cube(whiteMaterial, smallObject * transform.Translation(4, 0, 4)),
                    new shapes.Cube(purpleMaterial, mediumObject * transform.Translation(7.5, 0.5, 4)),
                    new shapes.Cube(whiteMaterial, mediumObject * transform.Translation(-0.25, 0.25, 8)),
                    new shapes.Cube(blueMaterial,  largeObject * transform.Translation(4, 1, 7.5)),
                    new shapes.Cube(redMaterial,  mediumObject * transform.Translation(10, 2, 7.5)),
                    new shapes.Cube(whiteMaterial,  smallObject * transform.Translation(8, 2, 12)),
                    new shapes.Cube(whiteMaterial,  smallObject * transform.Translation(20, 1, 9)),
                    new shapes.Cube(blueMaterial,  largeObject * transform.Translation(-0.5, -5, 0.25)),
                    new shapes.Cube(redMaterial,  largeObject * transform.Translation(4, -4, 0)),
                    new shapes.Cube(whiteMaterial,  largeObject * transform.Translation(8.5, -4, 0)),
                    new shapes.Cube(whiteMaterial,  largeObject * transform.Translation(0, -4, 4)),
                    new shapes.Cube(purpleMaterial,  largeObject * transform.Translation(-0.5, -4.5, 8)),
                    new shapes.Cube(whiteMaterial,  largeObject * transform.Translation(0, -8, 4)),
                    new shapes.Cube(whiteMaterial,  largeObject * transform.Translation(-0.5, -8.5, 8))
                }
            );


            var world = new RTF.World()
            {
                Lights = new List<RTF.Light>() { light1, light2 },

                Objects = new List<shapes.Shape>()
                {
                    group, whitebackdrop
                }
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

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = CreateWorld();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\Final[{elapsedMs}ms].ppm");
        }
    }
}
