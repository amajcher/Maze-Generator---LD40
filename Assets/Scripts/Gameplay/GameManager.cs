using Assets.Scripts.Procedural;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public Camera ViewCamera;
        public Transform Player;
        public GameObject WinPoint;

        private LevelGenerator _levelGenerator;
        private GameSettings _gameSettings;
        private GameObject _winPoints;
        private int _timeLeft;
        [Inject]
        public void Constructor(LevelGenerator levelGenerator, GameSettings gameSettings, string difficulty)
        {
            _levelGenerator = levelGenerator;
            _gameSettings = gameSettings;
            _winPoints = new GameObject("WinPoints");
            
        }
        public void Awake()
        {
            GenerateLevel();
        }
        public void GenerateLevel()
        {
            
            while (_winPoints.transform.childCount > 0)
                GameObject.DestroyImmediate(_winPoints.transform.GetChild(0).gameObject);

            _gameSettings.Active = false;
            Player.gameObject.SetActive(false);
            ViewCamera.gameObject.SetActive(true);
            _levelGenerator.Restert();

            _timeLeft = 5;
            _levelGenerator.Generate();
            StartCoroutine("StartCounting");
            
        }

        private void SetGameplayObjects()
        {
            _gameSettings.WinPointsCount = 0;
            _gameSettings.TimeLeft = _gameSettings.Time;
            
            StartCoroutine("TimerCounting");

            StartCoroutine("BuildingShow");

            Vector3 pos = _levelGenerator.FindPositionForPlayer() + new Vector3(0, 1.5f, 0);
            Player.position = pos;

            List<Vector3> winPointsPos = _levelGenerator.FindSetOfRandomWinPointsPositions(_gameSettings.WinPointsTotal);

            foreach (var p in winPointsPos)
            {
                var g = Instantiate(WinPoint) as GameObject;
                g.transform.position = p + new Vector3(0, 2, 0);
                g.transform.parent = _winPoints.transform;
            }
        }

        private IEnumerator BuildingShow()
        {
            while (_levelGenerator.IsBuilding() && _gameSettings.Active)
            {
                yield return new WaitForSeconds(.03f);

                int p = Random.Range(0, 100);
                if (p < _gameSettings.WinPointsCount * 90)
                {
                    _levelGenerator.InsertBuilding();
                }
            }
        }

        private IEnumerator TimerCounting()
        {
            while (_gameSettings.Active && _gameSettings.TimeLeft > 0 && _gameSettings.WinPointsCount < _gameSettings.WinPointsTotal)
            {
                yield return new WaitForSeconds(1.0f);

                _gameSettings.TimeLeft--;
            }
        }

        private IEnumerator StartCounting()
        {
            while (_timeLeft > 0 )
            {
                yield return new WaitForSeconds(1.0f);

                _timeLeft--;
            }

            ViewCamera.gameObject.SetActive(false);
            Player.gameObject.SetActive(true);

            _gameSettings.Active = true;
            _levelGenerator.HideAllBuildings();
            SetGameplayObjects();
        }
    }
}
