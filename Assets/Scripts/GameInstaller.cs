using Assets.Scripts.Gameplay;
using Assets.Scripts.Procedural;
using System;
using Zenject;

namespace Assets.Scripts
{
    public class GameInstaller :MonoInstaller
    {
        [InjectOptional]
        public string Difficulty = "easy";

        public override void InstallBindings()
        {
          Container.Bind<LevelGenerator>().AsSingle();
          Container.Bind<BuildingsGenerator>().AsSingle();
          Container.Bind<BuildingsViewGenerator>().AsSingle();
          Container.Bind<RoadsGenerator>().AsSingle();
          Container.Bind<RoadsViewGenerator>().AsSingle();
          //Container.BindInstance(Difficulty).WhenInjectedInto<GameManager>();


        }
    }
}
