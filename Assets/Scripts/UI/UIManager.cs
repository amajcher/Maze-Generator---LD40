using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Gameplay
{
    public class UIManager : MonoBehaviour
    {
        public GameObject Panel;
        public TMP_Text endGameText;

        public TMP_Text winPoints;
        public TMP_Text Timer;


        private GameSettings _gameSettings;

        [Inject]
        public void Constructor(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        void OnGUI()
        {
            winPoints.text = "<size=120%>" + _gameSettings.WinPointsCount + "/" + _gameSettings.WinPointsTotal + "</size>";
            Timer.text = "<size=120%>" + _gameSettings.TimeLeft  + "</size>";

            if (!_gameSettings.Active)
            {
                Timer.text = "";
                Panel.gameObject.SetActive(false);
            }else if (_gameSettings.TimeLeft <= 0)
            {
                _gameSettings.TimeLeft = 0;
                Panel.gameObject.SetActive(true);
                endGameText.text = "Game Over";
            }else if(_gameSettings.WinPointsCount == _gameSettings.WinPointsTotal)
            {
                Panel.gameObject.SetActive(true);
                endGameText.text = "Great!";
            }
            else
            {
                Panel.gameObject.SetActive(false);
            }

        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }


    }
}
