using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Procedural
{
    public class BuildingsViewGenerator
    {
        public List<GameObject> BuildingsView;

        private readonly BuildingsGenerator _BuildingsGenerator;
        private readonly GeneratorSettings _generationSettings;
        private readonly GameObject _Buildings;

        public BuildingsViewGenerator(GeneratorSettings generationSettings, BuildingsGenerator BuildingsGenerator)
        {
            _BuildingsGenerator = BuildingsGenerator;
            _generationSettings = generationSettings;

            _Buildings = new GameObject("Hosues");
        }

        public void Restert()
        {
            while (_Buildings.transform.childCount > 0)
                GameObject.DestroyImmediate(_Buildings.transform.GetChild(0).gameObject);

            BuildingsView = new List<GameObject>();
        }

        public void Generate()
        {
            Restert();

            if (!_generationSettings.AreBuildingsEnable)
                return;
            _BuildingsGenerator.Generate();

            for (int i = 0; i < _BuildingsGenerator.FinalBuildings.Count; i++)
            {
                GameObject Building = new GameObject("Building " + i);
                Building.tag = "Building";
                Building.transform.parent = _Buildings.transform;
                Building.AddComponent<MeshFilter>();
                Building.AddComponent<MeshRenderer>();

                List<Vector3> vertices;
                List<int> triangles;
                CreateFaces(out vertices, out triangles, _BuildingsGenerator.FinalBuildings[i]);

                var mesh = new Mesh();
                mesh.SetVertices(vertices);
                mesh.SetTriangles(triangles, 0);
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                Building.GetComponent<MeshFilter>().mesh = mesh;
                Building.GetComponent<MeshRenderer>().material = _generationSettings.BuildingBuildingMaterial;
                BuildingsView.Add(Building);
            }
        }

        private void CreateFaces(out List<Vector3> vertices, out List<int> triangles, Polygon Building)
        {
            List<Vector3> bottom = Building.GetVertices();
            bottom.Reverse();
            int height = Random.Range(_generationSettings.BuildingMinHeight, _generationSettings.BuildingMaxHeight);
            List<Vector3> top = bottom.Select(e => e + new Vector3(0, Random.Range(height - 1, height + 1), 0)).ToList();
            //top.Reverse();
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
    }
}