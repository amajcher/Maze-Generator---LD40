using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Procedural
{
    public class RoadsViewGenerator
    {
        private List<Edge> _roadsEdges;
        private List<Polygon> _roadsSegments;

        public List<Edge> Edges;
        public List<Vector3> Vertices;

        private readonly RoadsGenerator _roadsGenerator;
        private readonly GeneratorSettings _generationSettings;

        private readonly GameObject _roads;
        private readonly GameObject _segments;

        public RoadsViewGenerator(GeneratorSettings generationSettings, RoadsGenerator roadsGenerator)
        {
            _roadsGenerator = roadsGenerator;
            _generationSettings = generationSettings;

            _roads = new GameObject("Roads");
            _segments = new GameObject("Segments");
        }

        internal void Restert()
        {
            _roadsGenerator.Generate(new Polygon());

            _roadsEdges = _roadsGenerator.FinalRoads;
            _roadsSegments = _roadsGenerator.FinalSubdevisions;

            Vertices = new List<Vector3>();
            Edges = new List<Edge>();

            while (_segments.transform.childCount > 0)
                GameObject.DestroyImmediate(_segments.transform.GetChild(0).gameObject);
            while (_roads.transform.childCount > 0)
                GameObject.DestroyImmediate(_roads.transform.GetChild(0).gameObject);
        }

        public void Generate()
        {
            Restert();

            GenerateRoads();
            GenerateSegments();        }

        private void GenerateSegments()
        {
            foreach (var segment in _roadsSegments)
            {
                GameObject seg = new GameObject("Segment ");
                seg.tag = "Segment";
                seg.transform.parent = _segments.transform;
                var mf = seg.AddComponent<MeshFilter>();
                seg.AddComponent<MeshRenderer>();

                var verts = segment.GetVertices();
                var lastTop = verts.Count - 1;
                verts.AddRange(verts.Select(e => e + new Vector3(0, -2f, 0)).ToList());
                var lastBottom = verts.Count - 1;
                var tris = new List<int>();

                for (int i = 0; i < lastTop - 1; i++)
                {
                    tris.AddRange(new List<int>() {
                        i, i+1, lastTop, lastBottom,
                        lastTop + i +2, lastTop + i + 1,
                        i+1, i, lastTop + i +2,
                        lastTop + i +2, i, lastTop + i + 1
                    });
                }
                tris.AddRange(new List<int>()
                {
                   lastBottom, 0, lastTop,
                   lastTop +1, 0, lastBottom,
                   lastTop, lastTop-1, lastBottom,
                   lastBottom, lastTop-1, lastBottom-1
                });

                var mesh = new Mesh();
                mesh.SetVertices(verts);
                mesh.SetTriangles(tris, 0);
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                mf.mesh = mesh;
                seg.AddComponent<MeshCollider>();
                seg.GetComponent<MeshRenderer>().material = _generationSettings.SegmentMaterial;
            }
        }

        private void GenerateRoads()
        {
            foreach (var edge in _roadsEdges)
            {
                GameObject road = new GameObject("Road ");
                road.tag = "Road";
                road.transform.parent = _roads.transform;
                var mf = road.AddComponent<MeshFilter>();
                road.AddComponent<MeshRenderer>();

                List<Vector3> vertices;
                List<int> triangles;
                CreateFaces(out vertices, out triangles, edge);

                var mesh = new Mesh();
                mesh.SetVertices(vertices);
                mesh.SetTriangles(triangles, 0);
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                mf.mesh = mesh;
                road.AddComponent<MeshCollider>();
                road.GetComponent<MeshRenderer>().material = _generationSettings.RoadMaterial;
            }
        }

        private void CreateFaces(out List<Vector3> vertices, out List<int> triangles, Edge edge)
        {
            var ve = edge.V2 - edge.V1;
            var vee = Vector3.Normalize(Vector3.Cross(ve, _generationSettings.RoadWidth));

            var bottom = new List<Vector3>();
            bottom.Add(edge.V1 + vee);
            bottom.Add(edge.V2 + vee);
            bottom.Add(edge.V2 - vee);
            bottom.Add(edge.V1 - vee);
            var top = bottom.Select(e => e + new Vector3(0, 0.5f, 0)).ToList();

            Vertices.AddRange(top);
            Edges.Add(new Edge(top[0], top[1]));
            Edges.Add(new Edge(top[1], top[2]));
            Edges.Add(new Edge(top[2], top[3]));
            Edges.Add(new Edge(top[3], top[0]));

            vertices = new List<Vector3>() {
	            // Bottom
	            bottom[0], bottom[1], bottom[2], bottom[3],

	            // Left
	            top[3], top[0], bottom[0], bottom[3],

	            // Front
	            top[0], top[1], bottom[1], bottom[0],

	            // Back
	            top[2], top[3], bottom[3], bottom[2],

	            // Right
	            top[1], top[2], bottom[2], bottom[1],

	            // Top
	            top[3], top[2], top[1], top[0]
            };
            triangles = new List<int>()
            {
	            // Bottom
	            3, 1, 0,
                3, 2, 1,

	            // Left
	            3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
                3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,

	            // Front
	            3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
                3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

	            // Back
	            3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
                3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,

	            // Right
	            3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
                3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

	            // Top
	            3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
                3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
            };
        }
 /*
        public void OnDrawGizmos()
        {
           if (Vertices == null || _roadsSegments == null || _roadsEdges == null)
                return;

            var shift = new Vector3(100, 0, 0);

            Gizmos.color = Color.white;
            for (int i = 0; i < Vertices.Count; i += 4)
            {
                Gizmos.DrawLine(shift + Vertices[i], shift + Vertices[i + 1]);
                Gizmos.DrawLine(shift + Vertices[i + 1], shift + Vertices[i + 2]);
                Gizmos.DrawLine(shift + Vertices[i + 2], shift + Vertices[i + 3]);
                Gizmos.DrawLine(shift + Vertices[i + 3], shift + Vertices[i]);
            }

            Gizmos.color = Color.red;
            foreach (Edge e in _roadsEdges)
            {
                Gizmos.DrawLine(e.V1 - shift, e.V2 - shift);
            }
            Gizmos.color = Color.blue;
            foreach (Polygon p in _roadsSegments)
            {
                var c = p.GetCentroid();
                Gizmos.DrawSphere(c, .5f);
            }
        }*/
    }
}