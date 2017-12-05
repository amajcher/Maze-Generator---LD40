using UnityEngine;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Procedural;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    [InjectOptional]
    public string Difficulty = "easy";

    
    public GeneratorSettings GeneratorEasySettings;
    public GeneratorSettings GeneratorMediumSettings;
    public GeneratorSettings GeneratorHardSettings;
    public GameSettings GameSettings;

   

    public override void InstallBindings()
    {
        Container.BindInstances(GameSettings);
        switch (Difficulty)
        {
            case "easy":
                Container.BindInstances(GeneratorEasySettings);
                break;
            case "medium":
                Container.BindInstances(GeneratorMediumSettings);
                break;
            case "hard":
                Container.BindInstances(GeneratorHardSettings);
                break;
        }
        Container.BindInstance(Difficulty).WhenInjectedInto<GameManager>();
    }
}