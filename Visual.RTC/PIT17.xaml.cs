using System;
using System.Collections.Generic;
using System.Windows;
using RTF = RayTracerChallenge.Features;
using point = RayTracerChallenge.Features.PointType;
using transform = RayTracerChallenge.Helpers.Transformations;
using shapes = RayTracerChallenge.Features.Shapes;
using patterns = RayTracerChallenge.Features.Patterns;
using UV = RayTracerChallenge.Features.Patterns.UV;

namespace Visual.RTC
{
    /// <summary>
    /// Logique d'interaction pour PIT16.xaml
    /// </summary>
    public partial class PIT17 : Window
    {
        public PIT17()
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

        /* 
         *- add: camera
            width: 400
            height: 400
            field-of-view: 0.5
            from: [0, 0, -5]
            to: [0, 0, 0]
            up: [0, 1, 0]

        - add: light
            at: [-10, 10, -10]
            intensity: [1, 1, 1]

        - add: sphere
            material:
            pattern:
                type: map
                mapping: spherical
                uv_pattern:
                    type: checkers
                    width: 20
                    height: 10
                    colors:
                        - [0, 0.5, 0]
                        - [1, 1, 1]
            ambient: 0.1
            specular: 0.4
            shininess: 10
            diffuse: 0.6
         */
        private void CheckersOnSphere_Click(object sender, RoutedEventArgs e)
        {
            var camera = new RTF.Camera(400, 400, 0.5)
            {
                Transform = transform.ViewTransform(
                    point.Point(0, 0, -5),
                    point.Point(0, 0, 0),
                    point.Vector(0, 1, 0))
            };

            var light = new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White);

            var checker = new UV.Checker(20, 10, new RTF.Color(0, 0.5, 0), new RTF.Color(1, 1, 1));
            var pattern = new patterns.TextureMap(UV.Pattern.SphericalMap, checker);
            var sphere = new shapes.Sphere()
            {
                Material = new RTF.Material()
                {
                    Pattern = pattern,
                    Ambient = 0.1,
                    Specular = 0.4,
                    Shininess = 10,
                    Diffuse = 0.6
                }
            };

            var world = new RTF.World();
            world.Lights.Add(light);
            world.Objects.Add(sphere);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = RTF.Canvas.Render(camera, world);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\TextureMap_2DCheckers3DSphere[{elapsedMs}ms].ppm");
        }

        /* 
        - add: camera
          width: 400
          height: 400
          field-of-view: 0.5
          from: [1, 2, -5]
          to: [0, 0, 0]
          up: [0, 1, 0]

        - add: light
          at: [-10, 10, -10]
          intensity: [1, 1, 1]

        - add: plane
          material:
            pattern:
              type: map
              mapping: planar
              uv_pattern:
                type: checkers
                width: 2
                height: 2
                colors:
                  - [0, 0.5, 0]
                  - [1, 1, 1]
            ambient: 0.1
            specular: 0
            diffuse: 0.9
         */
        private void CheckersOnPlanar_Click(object sender, RoutedEventArgs e)
        {
            var camera = new RTF.Camera(400, 400, 0.5)
            {
                Transform = transform.ViewTransform(
                    point.Point(1, 2, -5),
                    point.Point(0, 0, 0),
                    point.Vector(0, 1, 0))
            };

            var light = new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White);

            var checker = new UV.Checker(2, 2, new RTF.Color(0, 0.5, 0), new RTF.Color(1, 1, 1));
            var pattern = new patterns.TextureMap(UV.Pattern.PlanarMap, checker);
            var plane = new shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Pattern = pattern,
                    Ambient = 0.1,
                    Specular = 0,
                    Diffuse = 0.9
                }
            };

            var world = new RTF.World();
            world.Lights.Add(light);
            world.Objects.Add(plane);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = RTF.Canvas.Render(camera, world);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\TextureMap_2DCheckersPlanar[{elapsedMs}ms].ppm");
        }

        /*
         *  - add: camera
              width: 400
              height: 400
              field-of-view: 0.5
              from: [0, 0, -10]
              to: [0, 0, 0]
              up: [0, 1, 0]

            - add: light
              at: [-10, 10, -10]
              intensity: [1, 1, 1]

            - add: cylinder
              min: 0
              max: 1
              transform:
                - [translate, 0, -0.5, 0]
                - [scale, 1, 3.1415, 1]
              material:
                pattern:
                  type: map
                  mapping: cylindrical
                  uv_pattern:
                    type: checkers
                    width: 16
                    height: 8
                    colors:
                      - [0, 0.5, 0]
                      - [1, 1, 1]
                ambient: 0.1
                specular: 0.6
                shininess: 15
                diffuse: 0.8
         * 
         */
        private void CheckersOnCylinder_Click(object sender, RoutedEventArgs e)
        {
            var camera = new RTF.Camera(400, 400, 0.5)
            {
                Transform = transform.ViewTransform(
                    point.Point(0, 0, -10),
                    point.Point(0, 0, 0),
                    point.Vector(0, 1, 0))
            };

            var light = new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White);

            var checker = new UV.Checker(16, 8, new RTF.Color(0, 0.5, 0), new RTF.Color(1, 1, 1));
            var pattern = new patterns.TextureMap(UV.Pattern.CylindricalMap, checker);
            var cylinder = new shapes.Cylinder()
            {
                Minimum = 0,
                Maximum = 1,
                Transform = transform.Translation(0, -0.5, 0) * transform.Scaling(1, Math.PI, 1),
                Material = new RTF.Material()
                {
                    Pattern = pattern,
                    Ambient = 0.1,
                    Specular = 0.6,
                    Shininess = 15,
                    Diffuse = 0.8
                }
            };

            var world = new RTF.World();
            world.Lights.Add(light);
            world.Objects.Add(cylinder);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = RTF.Canvas.Render(camera, world);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\TextureMap_2DCheckersCylinder[{elapsedMs}ms].ppm");
        }

        /*
         * - add: camera
              width: 400
              height: 400
              field-of-view: 0.5
              from: [1, 2, -5]
              to: [0, 0, 0]
              up: [0, 1, 0]

            - add: light
              at: [-10, 10, -10]
              intensity: [1, 1, 1]

            - add: plane
              material:
                pattern:
                  type: map
                  mapping: planar
                  uv_pattern:
                    type: align_check
                    colors:
                      main: [1, 1, 1] # white
                      ul: [1, 0, 0]   # red
                      ur: [1, 1, 0]   # yellow
                      bl: [0, 1, 0]   # green
                      br: [0, 1, 1]   # cyan
                ambient: 0.1
                diffuse: 0.8
         */
        private void AlignCheck_Click(object sender, RoutedEventArgs e)
        {
            var camera = new RTF.Camera(400, 400, 0.5)
            {
                Transform = transform.ViewTransform(
                    point.Point(1, 2, -5),
                    point.Point(0, 0, 0),
                    point.Vector(0, 1, 0))
            };

            var light = new RTF.Light(point.Point(-10, 10, -10), RTF.Color.White);

            var checker = new UV.AlignCheck(RTF.Color.White, RTF.Color.Red, RTF.Color.Yellow, RTF.Color.Lime, RTF.Color.Cyan);
            var pattern = new patterns.TextureMap(UV.Pattern.PlanarMap, checker);
            var cylinder = new shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Pattern = pattern,
                    Ambient = 0.1,
                    Diffuse = 0.8
                }
            };

            var world = new RTF.World();
            world.Lights.Add(light);
            world.Objects.Add(cylinder);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = RTF.Canvas.Render(camera, world);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\TextureMap_AlignCheck[{elapsedMs}ms].ppm");
        }
    }
}