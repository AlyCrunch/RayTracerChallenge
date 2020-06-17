using System;
using System.Collections.Generic;
using System.Windows;
using RTF = RayTracerChallenge.Features;
using point = RayTracerChallenge.Features.PointType;
using transform = RayTracerChallenge.Helpers.Transformations;
using shapes = RayTracerChallenge.Features.Shapes;
using patterns = RayTracerChallenge.Features.Patterns;
using UV = RayTracerChallenge.Features.Patterns.Map;
using System.IO;

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
            var plane = new shapes.Plane()
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
            world.Objects.Add(plane);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = RTF.Canvas.Render(camera, world);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\TextureMap_AlignCheck[{elapsedMs}ms].ppm");
        }

        /*
         * - add: camera
              width: 800
              height: 400
              field-of-view: 0.8
              from: [0, 0, -20]
              to: [0, 0, 0]
              up: [0, 1, 0]

            - add: light
              at: [0, 100, -100]
              intensity: [0.25, 0.25, 0.25]
            - add: light
              at: [0, -100, -100]
              intensity: [0.25, 0.25, 0.25]
            - add: light
              at: [-100, 0, -100]
              intensity: [0.25, 0.25, 0.25]
            - add: light
              at: [100, 0, -100]
              intensity: [0.25, 0.25, 0.25]

            - define: MappedCube
              value:
                add: cube
                material:
                  pattern:
                    type: map
                    mapping: cube
                    left:
                      type: align_check
                      colors:
                        main: [1, 1, 0] # yellow
                        ul: [0, 1, 1]   # cyan
                        ur: [1, 0, 0]   # red
                        bl: [0, 0, 1]   # blue
                        br: [1, 0.5, 0] # brown
                    front:
                      type: align_check
                      colors:
                        main: [0, 1, 1] # cyan
                        ul: [1, 0, 0]   # red
                        ur: [1, 1, 0]   # yellow
                        bl: [1, 0.5, 0] # brown
                        br: [0, 1, 0]   # green
                    right:
                      type: align_check
                      colors:
                        main: [1, 0, 0] # red
                        ul: [1, 1, 0]   # yellow
                        ur: [1, 0, 1]   # purple
                        bl: [0, 1, 0]   # green
                        br: [1, 1, 1]   # white
                    back:
                      type: align_check
                      colors:
                        main: [0, 1, 0] # green
                        ul: [1, 0, 1]   # purple
                        ur: [0, 1, 1]   # cyan
                        bl: [1, 1, 1]   # white
                        br: [0, 0, 1]   # blue
                    up:
                      type: align_check
                      colors:
                        main: [1, 0.5, 0] # brown
                        ul: [0, 1, 1]   # cyan
                        ur: [1, 0, 1]   # purple
                        bl: [1, 0, 0]   # red
                        br: [1, 1, 0]   # yellow
                    down:
                      type: align_check
                      colors:
                        main: [1, 0, 1] # purple
                        ul: [1, 0.5, 0] # brown
                        ur: [0, 1, 0]   # green
                        bl: [0, 0, 1]   # blue
                        br: [1, 1, 1]   # white
                  ambient: 0.2
                  specular: 0
                  diffuse: 0.8

            - add: MappedCube
              transform:
                - [rotate-y, 0.7854]
                - [rotate-x, 0.7854]
                - [translate, -6, 2, 0]

            - add: MappedCube
              transform:
                - [rotate-y, 2.3562]
                - [rotate-x, 0.7854]
                - [translate, -2, 2, 0]

            - add: MappedCube
              transform:
                - [rotate-y, 3.927]
                - [rotate-x, 0.7854]
                - [translate, 2, 2, 0]

            - add: MappedCube
              transform:
                - [rotate-y, 5.4978]
                - [rotate-x, 0.7854]
                - [translate, 6, 2, 0]

            - add: MappedCube
              transform:
                - [rotate-y, 0.7854]
                - [rotate-x, -0.7854]
                - [translate, -6, -2, 0]

            - add: MappedCube
              transform:
                - [rotate-y, 2.3562]
                - [rotate-x, -0.7854]
                - [translate, -2, -2, 0]

            - add: MappedCube
              transform:
                - [rotate-y, 3.927]
                - [rotate-x, -0.7854]
                - [translate, 2, -2, 0]

            - add: MappedCube
              transform:
                - [rotate-y, 5.4978]
                - [rotate-x, -0.7854]
                - [translate, 6, -2, 0]
         */
        private void CubeAlignCheck_Click(object sender, RoutedEventArgs e)
        {

            var camera = new RTF.Camera(800, 400, 0.8)
            {
                Transform = transform.ViewTransform(
                    point.Point(0, 0, -20),
                    point.Point(0, 0, 0),
                    point.Vector(0, 1, 0))
            };

            var light1 = new RTF.Light(point.Point(0, 100, -100), new RTF.Color(0.25, 0.25, 0.25));
            var light2 = new RTF.Light(point.Point(0, -100, -100), new RTF.Color(0.25, 0.25, 0.25));
            var light3 = new RTF.Light(point.Point(-100, 0, -100), new RTF.Color(0.25, 0.25, 0.25));
            var light4 = new RTF.Light(point.Point(100, 0, -100), new RTF.Color(0.25, 0.25, 0.25));

            var material = new RTF.Material()
            {
                Pattern = new UV.Cube(
                        new UV.AlignCheck(RTF.Color.Yellow, RTF.Color.Cyan, RTF.Color.Red, RTF.Color.Blue, RTF.Color.Brown),
                        new UV.AlignCheck(RTF.Color.Cyan, RTF.Color.Red, RTF.Color.Yellow, RTF.Color.Brown, RTF.Color.Green),
                        new UV.AlignCheck(RTF.Color.Red, RTF.Color.Yellow, RTF.Color.Purple, RTF.Color.Green, RTF.Color.White),
                        new UV.AlignCheck(RTF.Color.Green, RTF.Color.Purple, RTF.Color.Cyan, RTF.Color.White, RTF.Color.Blue),
                        new UV.AlignCheck(RTF.Color.Brown, RTF.Color.Cyan, RTF.Color.Purple, RTF.Color.Red, RTF.Color.Yellow),
                        new UV.AlignCheck(RTF.Color.Purple, RTF.Color.Brown, RTF.Color.Green, RTF.Color.Blue, RTF.Color.White)
                        ),
                Ambient = 0.2,
                Specular = 0,
                Diffuse = 0.8
            };

            var mappedCube1 = new shapes.Cube
                (material, transform.Translation(-6, 2, 0) * transform.RotationX(0.7854) * transform.RotationY(0.7854));
            var mappedCube2 = new shapes.Cube
                (material, transform.Translation(-2, 2, 0) * transform.RotationX(0.7854) * transform.RotationY(2.3562));
            var mappedCube3 = new shapes.Cube
                (material, transform.Translation(2, 2, 0) * transform.RotationX(0.7854) * transform.RotationY(3.927));
            var mappedCube4 = new shapes.Cube
                (material, transform.Translation(6, 2, 0) * transform.RotationX(0.7854) * transform.RotationY(5.4978));
            var mappedCube5 = new shapes.Cube
                (material, transform.Translation(-6, -2, 0) * transform.RotationX(-0.7854) * transform.RotationY(0.7854));
            var mappedCube6 = new shapes.Cube
                (material, transform.Translation(-2, -2, 0) * transform.RotationX(-0.7854) * transform.RotationY(2.3562));
            var mappedCube7 = new shapes.Cube
                (material, transform.Translation(2, -2, 0) * transform.RotationX(-0.7854) * transform.RotationY(3.927));
            var mappedCube8 = new shapes.Cube
                (material, transform.Translation(6, -2, 0) * transform.RotationX(-0.7854) * transform.RotationY(5.4978));


            var world = new RTF.World
            {
                Lights = new List<RTF.Light>() { light1, light2, light3, light4 },
                Objects = new List<shapes.Shape>() { mappedCube1, mappedCube2, mappedCube3, mappedCube4, mappedCube5, mappedCube6, mappedCube7, mappedCube8 }
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var canvas = RTF.Canvas.Render(camera, world);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            canvas.SaveAsPPMFile(FolderPath.Text + $"\\TextureMap_CubeAlignCheck[{elapsedMs}ms].ppm");
        }

        private void Earth_Click(object sender, RoutedEventArgs e)
        {
            var file = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(@".\Files\EarthMap.ppm"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        file.Add(line);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }

            var canvas = RTF.Canvas.CanvasFromPPM(file);
            var pattern = new UV.Image(canvas);

            var light = new RTF.Light(point.Point(-100, 100, -100), RTF.Color.White);

            var camera = new RTF.Camera(800, 400, 0.8)
            {
                Transform = transform.ViewTransform(
                    point.Point(1, 2, -10),
                    point.Point(0, 1.1, 0),
                    point.Vector(0, 1, 0))
            };

            var cylinder = new shapes.Cylinder()
            {
                Minimum = 0,
                Maximum = 0.1,
                Closed = true,
                Material = new RTF.Material()
                {
                    Color = RTF.Color.White,
                    Diffuse = 0.2,
                    Specular = 0,
                    Ambient = 0,
                    Reflective = 0.1
                }
            };

            var plane = new shapes.Plane()
            {
                Material = new RTF.Material()
                {
                    Color = RTF.Color.White,
                    Diffuse = 0.1,
                    Specular = 0,
                    Ambient = 0,
                    Reflective = 0.4
                }
            };

            var sphere = new shapes.Sphere()
            {
                Transform = transform.Translation(0, 1.1, 0) * transform.RotationY(1.9),
                Material = new RTF.Material()
                {
                    Pattern = new patterns.TextureMap(UV.Pattern.SphericalMap, pattern),
                    Diffuse = 0.9,
                    Specular = 0.1,
                    Shininess = 10,
                    Ambient = 0.1
                }
            };

            var world = new RTF.World
            {
                Lights = new List<RTF.Light>() { light },
                Objects = new List<shapes.Shape>() { cylinder, plane, sphere }
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var finalCanvas = RTF.Canvas.Render(camera, world);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            finalCanvas.SaveAsPPMFile(FolderPath.Text + $"\\TextureMap_Earth[{elapsedMs}ms].ppm");
        }

        private void LancellottiChapel_Click(object sender, RoutedEventArgs e)
        {

            var light = new RTF.Light(point.Point(0, 100, 0), RTF.Color.White);

            var camera = new RTF.Camera(800, 400, 1.2)
            {
                Transform = transform.ViewTransform(
                    point.Point(0, 0, 0),
                    point.Point(0, 0, 5),
                    point.Vector(0, 1, 0))
            };

            var sphere = new shapes.Sphere()
            {
                Transform = transform.Translation(0, 0, 5) * transform.Scaling(0.75, 0.75, 0.75),
                Material = new RTF.Material()
                {
                    Diffuse = 0.4,
                    Specular = 0.6,
                    Shininess = 20,
                    Reflective = 0.6,
                    Ambient = 0
                }
            };
            var negx = new List<string>();
            var negy = new List<string>();
            var negz = new List<string>();
            var posx = new List<string>();
            var posy = new List<string>();
            var posz = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(@".\Files\negx.ppm"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null) { negx.Add(line); }
                }
                using (StreamReader sr = new StreamReader(@".\Files\negy.ppm"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null) { negy.Add(line); }
                }
                using (StreamReader sr = new StreamReader(@".\Files\negz.ppm"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null) { negz.Add(line); }
                }
                using (StreamReader sr = new StreamReader(@".\Files\posx.ppm"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null) { posx.Add(line); }
                }
                using (StreamReader sr = new StreamReader(@".\Files\posy.ppm"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null) { posy.Add(line); }
                }
                using (StreamReader sr = new StreamReader(@".\Files\posz.ppm"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null) { posz.Add(line); }
                }

            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }

            var pattern = new UV.Cube(new UV.Image(RTF.Canvas.CanvasFromPPM(negx)),
                                      new UV.Image(RTF.Canvas.CanvasFromPPM(posx)),
                                      new UV.Image(RTF.Canvas.CanvasFromPPM(posz)),
                                      new UV.Image(RTF.Canvas.CanvasFromPPM(negz)),
                                      new UV.Image(RTF.Canvas.CanvasFromPPM(posy)),
                                      new UV.Image(RTF.Canvas.CanvasFromPPM(negy)));

            var cube = new shapes.Sphere()
            {
                Transform = transform.Scaling(1000, 1000, 1000),
                Material = new RTF.Material()
                {
                    Pattern = pattern,
                    Diffuse = 0,
                    Specular = 0,
                    Ambient = 1
                }
            };

            var world = new RTF.World
            {
                Lights = new List<RTF.Light>() { light },
                Objects = new List<shapes.Shape>() { sphere, cube }
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var finalCanvas = RTF.Canvas.Render(camera, world);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            finalCanvas.SaveAsPPMFile(FolderPath.Text + $"\\TextureMap_LancellottiChapel[{elapsedMs}ms].ppm");
        }
    }
}