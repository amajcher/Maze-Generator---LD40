using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Procedural
{
    [System.Serializable]
    public class GeneratorSettings 
    {
        public List<Edge> Eng = new List<Edge>();

        public int Size = 100;

        public float RoadMinArea = 30f;
        public float RoadMaxArea = 1450f;
        public float RoadMin = 0f;
        public float RoadMax = 1f;
        public float RoadRatio = 6f;
        public float RoadMinAngle = 35f;

        public Material RoadMaterial;
        public Material SegmentMaterial;
        public Vector3 RoadWidth = new Vector3(0, 2, 0);

        public GameObject RoadGenerationContainer;
        public bool AreBuildingsEnable = true;

        public int BuildingsDensity = 200;
        public int BuildingMaxEdgeSize = 5;

        public int BuildingMaxHeight = 25;
        public int BuildingMinHeight = 10;
        public Material BuildingBuildingMaterial;

        


    }
}
