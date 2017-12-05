using Zenject;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Gameplay
{
    public class Player : MonoBehaviour
    {
        private GameSettings _gameSettings;

        [Inject]
        public void Constructor(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        private void Start()
        {
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.tag == "WinPoint")
            {
                _gameSettings.WinPointsCount += 1;
                if(_gameSettings.WinPointsCount == _gameSettings.WinPointsTotal)
                    Debug.Log("WIN");
                Destroy(hit.gameObject);
            }else if (hit.gameObject.tag != "Road")
            {
                
                _gameSettings.TimeLeft = 0;
               
            }
            
        }
        private void Update()
        {
            if (_gameSettings.TimeLeft <= 0 || _gameSettings.WinPointsCount == _gameSettings.WinPointsTotal)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                transform.GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
                transform.GetComponent<FirstPersonController>().enabled = false;
            }
            else
            {
                transform.GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
                transform.GetComponent<FirstPersonController>().enabled = true;
            }
        }

    }
}