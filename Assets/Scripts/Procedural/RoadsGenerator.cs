using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Procedural
{
    public class RoadsGenerator
    {
        // Use this for initialization
        public readonly List<Polygon> FinalSubdevisions;
        public readonly List<Edge> FinalRoads;

        private Polygon _startingPoligon;

        private readonly Queue<Polygon> _oversizedSubdevisions;
        private GeneratorSettings _generatorSettings;


        public RoadsGenerator(GeneratorSettings generatorSettings)
        {
            _generatorSettings = generatorSettings;

            _oversizedSubdevisions = new Queue<Polygon>();
            FinalSubdevisions = new List<Polygon>();
            FinalRoads = new List<Edge>();
        }
        
        public List<Polygon> Generate(Polygon startingPoligon)
        {
            var s = _generatorSettings.Size;
            var hsize = s / 2;
            _startingPoligon = new Polygon()
            {
                Edges = new List<Edge>()
                {
                    new Edge(new Vector3(-hsize,0,-hsize), new Vector3(-hsize,0,hsize)),
                    new Edge(new Vector3(-hsize,0,hsize), new Vector3(hsize,0,hsize)),
                    new Edge(new Vector3(hsize,0,hsize), new Vector3(hsize,0,-hsize)),
                    new Edge(new Vector3(hsize,0,-hsize), new Vector3(-hsize,0,-hsize)),
                }
            };

            SetupGenerator();

            while (_oversizedSubdevisions.Count != 0)
            {
                var workingPolygon = _oversizedSubdevisions.Dequeue();
                var splitedPolygons = CreateValidSubdevisions(workingPolygon);
                AssignPolygonsBasedOnRestrictions(splitedPolygons);
            }
            return FinalSubdevisions;
        }

        private void SetupGenerator()
        {
            _oversizedSubdevisions.Clear();
            FinalSubdevisions.Clear();
            FinalRoads.Clear();
            _oversizedSubdevisions.Enqueue(_startingPoligon);
        }

        private List<Polygon> CreateValidSubdevisions(Polygon workingPolygon)
        {
            Edge bisectingLine;
            List<Polygon> splitedPolygons;

            do
            {
                var indexes = workingPolygon.GetRandomBisectiongIndexes();
                bisectingLine = CreateBisectiongLine(workingPolygon, indexes);
                splitedPolygons = SplitPolygons(workingPolygon, indexes, bisectingLine);
            } while (!AreSubdevisionsValid(splitedPolygons));

            FinalRoads.Add(bisectingLine);

            return splitedPolygons;
        }

        private Edge CreateBisectiongLine(Polygon workingPolygon, int[] indexes)
        {
            var edge1 = workingPolygon.Edges[indexes[0]];
            var edge2 = workingPolygon.Edges[indexes[1]];

            Vector3 d1 = Vector3.Lerp(edge1.V1, edge1.V2, Random.Range(_generatorSettings.RoadMin, _generatorSettings.RoadMax));
            Vector3 d2 = Vector3.Lerp(edge2.V1, edge2.V2, Random.Range(_generatorSettings.RoadMin, _generatorSettings.RoadMax));
            
            //TODO: Connect to existing roads when its near
            //TODO: Add curve to biger ones
            return new Edge(d1, d2);
        }

        private bool AreSubdevisionsValid(List<Polygon> splitedPolygons)
        {
            foreach (var polygon in splitedPolygons)
            {
                var ratio = polygon.DiameterWidthAspectRatio();

                float angle = polygon.GetSmallestAngle();
               
                if (polygon.Area() < _generatorSettings.RoadMinArea || ratio > _generatorSettings.RoadRatio || angle < _generatorSettings.RoadMinAngle)
                    return false;
            }

            return true;
        }

        private static List<Polygon> SplitPolygons(Polygon polygon, int[] edgeIndexes, Edge bisectingLine)
        {
            var subPolygon1 = new Polygon();
            var subPolygon2 = new Polygon();

            for (int i = 0; i < polygon.Edges.Count; i++)
            {
                var currentEdge = polygon.Edges[i];

                if (i == edgeIndexes[0])
                {
                    subPolygon1.Edges.Add(new Edge(currentEdge.V1, bisectingLine.V1));
                    subPolygon1.Edges.Add(new Edge(bisectingLine.V1, bisectingLine.V2));
                    subPolygon2.Edges.Add(new Edge(bisectingLine.V1, currentEdge.V2));
                }
                else if (i == edgeIndexes[1])
                {
                    subPolygon2.Edges.Add(new Edge(currentEdge.V1, bisectingLine.V2));
                    subPolygon2.Edges.Add(new Edge(bisectingLine.V2, bisectingLine.V1));
                    subPolygon1.Edges.Add(new Edge(bisectingLine.V2, currentEdge.V2));
                }
                else if (i > edgeIndexes[0] && i < edgeIndexes[1])
                {
                    subPolygon2.Edges.Add(currentEdge);
                }
                else
                {
                    subPolygon1.Edges.Add(currentEdge);
                }
            }

            return new List<Polygon>() { subPolygon1, subPolygon2 };
        }

        private void AssignPolygonsBasedOnRestrictions(List<Polygon> polygons)
        {
            foreach (var p in polygons)
            {
                if (p.Area() < _generatorSettings.RoadMaxArea)
                    FinalSubdevisions.Add(p);
                else
                    _oversizedSubdevisions.Enqueue(p);
            }
        }

        #region Debug

        public void OnDrawGizmos()
        {
           /* var er = new Edge(new Vector3(23, 0, 0), new Vector3(0, 0, 100));
            var ab = er.V2 - er.V1;
            var d = er.V1 + Vector3.Normalize(Vector3.Cross(ab, new Vector3(0, 2, 0)));
            Debug.Log("aaaa" + d);

            Gizmos.DrawLine(er.V1, er.V2);
            Gizmos.DrawLine(er.V1, d);



            if (FinalSubdevisions == null)
                return;
            if (FinalRoads == null)
                return;
            // Debug.Log(_finalSubdevisions.Count);
            Gizmos.color = Color.red;
            foreach (Edge e in FinalRoads)
            {
                Gizmos.DrawLine(e.V1, e.V2);
            }
            /* for (int i = 0; i < _finalSubdevisions.Count; i++)
             {
                 //Gizmos.color = Random.ColorHSV();
                 foreach (Edge e in _finalSubdevisions[i].Edges)
                     Gizmos.DrawLine(e.V1, e.V2);
             }*/
        }
        #endregion
    }
}