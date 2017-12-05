using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.Gameplay
{
    public class UILevelSelectionManager : MonoBehaviour
    {
        private ZenjectSceneLoader _sceneLoader;

        [Inject]
        public void Constructor(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void OnEasyPlay()
        {
            LoadNext("easy");
        }

        public void OnMediumPlay()
        {
            LoadNext("medium");
        }

        public void OnHardPlay()
        {
            LoadNext("hard");
        }

        public void LoadNext(string s)
        {
            _sceneLoader.LoadScene("Game", LoadSceneMode.Single, (container) =>
            {
                container.BindInstance(s).WhenInjectedInto<SettingsInstaller>();
            });
        }
    }
}