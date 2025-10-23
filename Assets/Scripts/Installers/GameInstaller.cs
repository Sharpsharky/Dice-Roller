using DiceRollingGame.Dice;
using DiceRollingGame.Input;
using DiceRollingGame.Roller;
using DiceRollingGame.Score;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DiceRollingGame.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Managers")]
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private SelectManager selectManager;
        [SerializeField] private DiceManager diceManager;

        [Header("Controllers")]
        [SerializeField] private Camera cam;
        [SerializeField] private DiceRollingController diceRollingController;
        [SerializeField] private InterfaceUIView interfaceUIView;
        [SerializeField] private List<DiceController> diceController;


        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(cam).AsSingle();
            Container.Bind<IRoller>().To<DiceRollingController>().FromInstance(diceRollingController).AsSingle().NonLazy(); //TODO: nonlazy?
            Container.Bind<InterfaceUIView>().FromInstance(interfaceUIView).AsSingle();

            foreach (var die in diceController)
            {
                Container.Bind<IDice>().To<DiceController>().FromInstance(die).AsCached();
            }

            Container.BindInterfacesTo<ScoreManager>().FromInstance(scoreManager).AsSingle();
            Container.BindInterfacesTo<SelectManager>().FromInstance(selectManager).AsSingle();
            Container.BindInterfacesTo<DiceManager>().FromInstance(diceManager).AsSingle();
        }
    }
}