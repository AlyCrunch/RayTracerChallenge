using RayTracerChallenge.Features;
using RayTracerChallenge.Features.Shapes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using pt = RayTracerChallenge.Features.PointType;

namespace RayTracerChallenge.Helpers
{
    public class FileParser
    {
        const int based = 1;
        const string defaultG = "DefaultGroup";
        public int IgnoredLines { get; set; }
        public List<pt> Vertices { get; set; } = new List<pt>();
        public List<pt> Normals { get; set; } = new List<pt>();
        public Group DefaultGroup => Groups[defaultG];
        public Dictionary<string, Group> Groups { get; set; }
            = new Dictionary<string, Group>() { { defaultG, new Group() } };
        public string LastGroupAdded { get; set; } = defaultG;

        public void Parse(string url)
        {
            List<string> list = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(url))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            Parse(list.ToArray());
        }
        public void Parse(string[] lines)
        {
            foreach (var line in lines)
            {
                if (line == string.Empty) continue;
                var splittedline = line.Split(new char[] { ' ' }, 2);
                switch (splittedline[0])
                {
                    case "v":
                        Vertices.Add(StringToPoint(splittedline[1]));
                        break;
                    case "vn":
                        Normals.Add(StringToVector(splittedline[1]));
                        break;
                    case "g":
                        LastGroupAdded = splittedline[1];
                        Groups.Add(LastGroupAdded, new Group());
                        break;
                    case "f":
                        Groups[LastGroupAdded].Add(
                        FanTriangulate(splittedline[1]));
                        break;
                    default: IgnoredLines++; break;
                }
            }
        }

        public Group ObjToGroup()
        {
            return new Group(Groups.Select(x => x.Value).ToList());
        }
        private pt StringToPoint(string str)
        {
            var pointStr = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return pt.Point(
                double.Parse(pointStr[0], CultureInfo.InvariantCulture),
                double.Parse(pointStr[1], CultureInfo.InvariantCulture),
                double.Parse(pointStr[2], CultureInfo.InvariantCulture));
        }
        private pt StringToVector(string str)
        {
            var pointStr = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return pt.Vector(
                double.Parse(pointStr[0], CultureInfo.InvariantCulture),
                double.Parse(pointStr[1], CultureInfo.InvariantCulture),
                double.Parse(pointStr[2], CultureInfo.InvariantCulture));
        }
        private List<Triangle> FanTriangulate(string str)
        {
            var tStr = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var triangles = new List<Triangle>();
            for (int i = 1; i < tStr.Length - 1; i++)
            {
                var origin = GetSettings(tStr[0]);
                var first = GetSettings(tStr[i]);
                var second = GetSettings(tStr[i + 1]);
                triangles.Add(
                    new Triangle(
                    Vertices[origin.index],
                    Vertices[first.index],
                    Vertices[second.index])
                    {
                        N1 = (origin.normal.HasValue)? Normals[origin.normal.Value] : null,
                        N2 = (first.normal.HasValue)? Normals[first.normal.Value] : null,
                        N3 = (second.normal.HasValue)? Normals[second.normal.Value] : null,
                        IsSmoothed = origin.normal.HasValue
                    }
                    );
            }

            return triangles;
        }
        private (int index, int? texture, int? normal) GetSettings(string str)
        {
            if (!str.Contains("/"))
                return (int.Parse(str) - based, null, null);
            var tStr = str.Split(new char[] { '/' });
            var index = int.Parse(tStr[0]) - based;
            int? texture = (tStr[1] == string.Empty) ? null : (int?)int.Parse(tStr[1]) - based;
            int? normal = (tStr[2] == string.Empty) ? null : (int?)int.Parse(tStr[2]) - based;
            return (index, texture, normal);
        }
    }
}
