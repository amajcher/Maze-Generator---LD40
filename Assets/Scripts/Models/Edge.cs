using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [System.Serializable]
    public struct Edge
    {
        public readonly Vector3 V1;
        public readonly Vector3 V2;
        public readonly float Lenght;

        public Edge(Vector3 v1, Vector3 v2)
        {
            V1 = v1;
            V2 = v2;
            Lenght = Vector3.Distance(V1, V2);
        }

       /* public float Lenght()
        {
            return Vector3.Distance(V1, V2);
        }*/

    }
}
