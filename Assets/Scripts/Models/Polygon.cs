using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Polygon
    {
        public List<Edge> Edges { get; set; }

        public Polygon()
        {
            Edges = new List<Edge>();
        }

        public int GetRandomEdgeIndex()
        {
            return Random.Range(0, (int)(Edges.Count));
        }

        public List<Vector3> GetVertices()
        {
            var v = new List<Vector3>();

            foreach (var edge in Edges)
            {
                v.Add(edge.V1);
            }

            return v;
        }

        public float Area()
        {
            var vertices = GetVertices();

            int i, j;
            float area = 0;

            for (i = 0; i < vertices.Count; i++)
            {
                j = (i + 1) % vertices.Count;

                area += vertices[i].x * vertices[j].z;
                area -= vertices[i].z * vertices[j].x;
            }

            area /= 2;
            return (area < 0 ? -area : area);
        }

        public float DiameterWidthAspectRatio()
        {
            Edges.Max(e => e.Lenght);
            float longEdge = Edges.Max(e => e.Lenght);
            float shortEdge = Edges.Min(e => e.Lenght);

            return longEdge / shortEdge;
        }

        public int GetRandomEdgeIndex(ref int previusEdgeIndex)
        {
            int newEdgeIndex = previusEdgeIndex;

            while (previusEdgeIndex == newEdgeIndex)
            {
                newEdgeIndex = GetRandomEdgeIndex();
            }

            if (previusEdgeIndex > newEdgeIndex)
            {
                int p = previusEdgeIndex;
                previusEdgeIndex = newEdgeIndex;
                newEdgeIndex = p;
            }

            return newEdgeIndex;
        }

        public int[] GetRandomBisectiongIndexes()
        {
            int e1 = GetRandomEdgeIndex();
            int e2 = e1;
            while (e1 == e2)
            {
                e2 = GetRandomEdgeIndex();
            }

            if (e1 > e2)
            {
                int p = e1;
                e1 = e2;
                e2 = p;
            }
            return new int[] { e1, e2 };
        }

        public float GetSmallestAngle()
        {
            float angle = float.MaxValue;
            int k = 1;
            for (int i = 0; i < Edges.Count; i++, k++)
            {
                if (k == Edges.Count)
                    k = 0;

                var a = Edges[i].V2 - Edges[i].V1;
                var b = Edges[k].V1 - Edges[k].V2;
                
                var edgeAngle = Vector3.Angle(a, b);

                if (edgeAngle < angle)
                    angle = edgeAngle;
            }

            return angle;
        }
        
        public Vector3 GetCentroid()
        {
            var vertices = GetVertices();

            float accumulatedArea = 0.0f;
            float centerX = 0.0f;
            float centerY = 0.0f;

            for (int i = 0, j = vertices.Count - 1; i < vertices.Count; j = i++)
            {
                float temp = vertices[i].x * vertices[j].z - vertices[j].x * vertices[i].z;
                accumulatedArea += temp;
                centerX += (vertices[i].x + vertices[j].x) * temp;
                centerY += (vertices[i].z + vertices[j].z) * temp;
            }

            if (Mathf.Abs(accumulatedArea) < 1E-7f)
                return Vector3.zero;  // Avoid division by zero

            accumulatedArea *= 3f;
            return new Vector3(centerX / accumulatedArea,0, centerY / accumulatedArea);
        }


        public bool IsPointInPolygon(Vector3 point)
        {
            var vertices = GetVertices();
            int polygonLength = vertices.Count, i = 0;
            bool inside = false;
            // x, y for tested point.
            float pointX = point.x, pointZ = point.z;
            // start / end point for the current polygon segment.
            float startX, startZ, endX, endZ;
            Vector3 endPoint = vertices[polygonLength - 1];
            endX = endPoint.x;
            endZ = endPoint.z;
            while (i < polygonLength)
            {
                startX = endX; startZ = endZ;
                endPoint = vertices[i++];
                endX = endPoint.x; endZ = endPoint.z;
                //
                inside ^= (endZ > pointZ ^ startZ > pointZ) /* ? pointY inside [startY;endY] segment ? */
                          && /* if so, test if it is under the segment */
                          ((pointX - endX) < (pointZ - endZ) * (startX - endX) / (startZ - endZ));
            }
            return inside;
        }












    }
}