using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Procedural
{
    public class BuildingsGenerator
    {
        public List<Polygon> FinalBuildings;
        public List<Vector3> Places;

        private readonly RoadsGenerator _roadsGenerator;
        private readonly GeneratorSettings _generationSettings;
        private readonly RoadsViewGenerator _roadsViewGenerator;

        public BuildingsGenerator(GeneratorSettings generationSettings, RoadsGenerator roadsGenerator, RoadsViewGenerator roadsViewGenerator)
        {
            _roadsGenerator = roadsGenerator;
            _generationSettings = generationSettings;
            _roadsViewGenerator = roadsViewGenerator;
        }

        public void Generate()
        {
            Places = new List<Vector3>();
            FinalBuildings = new List<Polygon>();

            GenerateRandomPointsInside();
        }

        private void GenerateRandomPointsInside()
        {
            bool isOut;

            for (int i = 0; i < _generationSettings.BuildingsDensity; i++)
            {
                Vector3 pp = new Vector3(Random.Range(-_generationSettings.Size / 2 + 3, _generationSettings.Size / 2 - 3), 0,
                    Random.Range(3 - _generationSettings.Size / 2, -3 + _generationSettings.Size / 2));
                isOut = false;

                Polygon Building = CreateBuilding(pp);

                foreach (var BuildingEdge in Building.Edges)
                {
                    if (ChcekRoudsColisons(BuildingEdge) || CheckBuildingColisions(BuildingEdge))
                    {
                        isOut = true;
                        break;
                    }
                }
                if (!isOut)
                {
                    FinalBuildings.Add(Building);
                }
            }
        }

        private bool ChcekRoudsColisons(Edge BuildingEdge)
        {
            foreach (var roadEdge in _roadsViewGenerator.Edges)
            {
                if (Math3d.AreLineSegmentsCrossing(roadEdge.V1, roadEdge.V2, BuildingEdge.V1, BuildingEdge.V2))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckBuildingColisions(Edge BuildingEdge)
        {
            foreach (var h in FinalBuildings)
            {
                var currentBuilding = h.GetVertices();
                if (Math3d.IsLineInRectangle(BuildingEdge.V1, BuildingEdge.V2, currentBuilding[0], currentBuilding[1], currentBuilding[2], currentBuilding[3]))
                {
                    return true;
                }
                foreach (var exEdge in h.Edges)
                {
                    if (Math3d.AreLineSegmentsCrossing(exEdge.V1, exEdge.V2, BuildingEdge.V1, BuildingEdge.V2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Polygon CreateBuilding(Vector3 pp)
        {
            var Building = new Polygon();

            Quaternion rot = Quaternion.Euler(0, Random.Range(1, 35), 0);

            float edge = Random.Range(1, _generationSettings.BuildingMaxEdgeSize);
            float edge2 = Random.Range(1, _generationSettings.BuildingMaxEdgeSize);

            Vector3 a = pp - (rot * new Vector3(-edge, 0, -edge2));
            Vector3 b = pp - (rot * new Vector3(edge, 0, -edge2));
            Vector3 c = pp - (rot * new Vector3(edge, 0, edge2));
            Vector3 d = pp - (rot * new Vector3(-edge, 0, edge2));

            Building.Edges.Add(new Edge(a, b));
            Building.Edges.Add(new Edge(b, c));
            Building.Edges.Add(new Edge(c, d));
            Building.Edges.Add(new Edge(d, a));

            return Building;
        }

        public void OnDrawGizmos()
        {
            /* if (points == null || FinalBuildings == null)
                 return;

             Gizmos.color = Color.yellow;
             foreach (var h in FinalBuildings)
             {
                 var verts = h.GetVertices();
                 for (int i = 0; i < verts.Count; i += 4)
                 {
                     Gizmos.DrawLine(verts[i], verts[i + 1]);
                     Gizmos.DrawLine(verts[i + 1], verts[i + 2]);
                     Gizmos.DrawLine(verts[i + 2], verts[i + 3]);
                     Gizmos.DrawLine(verts[i + 3], verts[i]);
                 }
             }

             var shift = new Vector3(100, 0, 0);
             Gizmos.color = Color.green;
             for (int i = 0; i < points.Count; i++)
             {
                 Gizmos.DrawSphere(points[i], .5f);
                 /*
                 * point is center
                 * create rectangles
                 * is rectangle insight rec or roads
                 * place rectagles
                 *
             }*/
        }
    }
}