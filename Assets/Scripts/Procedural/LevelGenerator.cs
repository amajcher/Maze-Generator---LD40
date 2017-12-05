using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Procedural
{
    public class LevelGenerator
    {
        private readonly RoadsViewGenerator _roadsViewGenerator;
        private readonly BuildingsViewGenerator _BuildingsViewGenerator;
        private readonly RoadsGenerator _roadsGenerator;
        private readonly GeneratorSettings _generatorSettings;

        public LevelGenerator(GeneratorSettings generatorSettings, RoadsViewGenerator roadsViewGenerator, BuildingsViewGenerator BuildingsViewGenerator, RoadsGenerator roadsGenerator)
        {
            _roadsViewGenerator = roadsViewGenerator;
            _BuildingsViewGenerator = BuildingsViewGenerator;
            _roadsGenerator = roadsGenerator;
            _generatorSettings = generatorSettings;
        }

        public void Generate()
        {
            _roadsViewGenerator.Generate();
            _BuildingsViewGenerator.Generate();
        }

        internal void Restert()
        {
            _roadsViewGenerator.Restert();
            _BuildingsViewGenerator.Restert();
        }

        public void HideAllBuildings()
        {
            foreach (var h in _BuildingsViewGenerator.BuildingsView)
            {
                h.transform.position -= new Vector3(0, 100, 0);
            }
        }

        public void InsertBuilding()
        {
            var n = Random.Range(0, _BuildingsViewGenerator.BuildingsView.Count - 1);
            var h = _BuildingsViewGenerator.BuildingsView[n];
            _BuildingsViewGenerator.BuildingsView.RemoveAt(n);

            h.transform.position += new Vector3(0, 100, 0);
        }

        public List<Vector3> FindSetOfRandomWinPointsPositions(int total)
        {
            var lines = _roadsGenerator.FinalRoads;
            var li = new List<Vector3>();
            var liIndexes = new List<int>();
            int actIndex = -1;

            for (int i = 0; i < total; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    actIndex = Random.Range(2, lines.Count - 1);
                    if (!liIndexes.Contains(actIndex))
                        break;
                }
                liIndexes.Add(actIndex);
                var line = lines[actIndex];
                li.Add(Vector3.Lerp(line.V1, line.V2, Random.Range(0.1f, 0.9f)));
            }
            return li;
        }

        internal bool IsBuilding()
        {
            return _BuildingsViewGenerator.BuildingsView.Count != 0;
        }

        public Vector3 FindPositionForPlayer()
        {
            var lines = _roadsGenerator.FinalRoads;
            int lineIndex = Random.Range(2, lines.Count - 1);
            var line = lines[lineIndex];

            return Vector3.Lerp(line.V1, line.V2, Random.Range(0.1f, 0.9f));
        }
    }
}