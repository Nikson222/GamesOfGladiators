using System;
using Elements;
using Field;
using MainMenu;
using Player;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Installers
{
    public class FieldInstaller : MonoInstaller
    {
        [SerializeField] private FieldSizeDB _fieldSizeDB;
        [SerializeField] private ElementsSettingsDB _elementsSettingsDB;
        [SerializeField] private MainMenuPanel _mainMenuPanel;
        [SerializeField] private GameplayPanel _gameplayPanel;
        [SerializeField] private PanelsSwitcher _panelsSwitcher;
        [SerializeField] private GameFieldParent _gameFieldParent;
        [SerializeField] private FieldView _gameFieldView;
        [SerializeField] private PlayerView _playerView;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            LastAddedBalanceDate lastAddedBalanceDate = new LastAddedBalanceDate();
            
            var savedBalance = PlayerPrefs.GetInt("Balance");
            
            if(lastAddedBalanceDate.IsFirstReceivedDay)
                savedBalance += 10000;
            
            if (!lastAddedBalanceDate.IsFirstReceivedDay && lastAddedBalanceDate.LastReceivedData != DateTime.Today)
                savedBalance += 10000;
            
            PlayerPrefs.SetInt("Balance", savedBalance);

            
            Container.Bind<IFactory<FieldInstanceData>>().To<FieldFactory>().AsSingle().NonLazy();

            Container.Bind<FieldSizeDB>().FromInstance(_fieldSizeDB).AsSingle();
            Container.Bind<ElementsSettingsDB>().FromInstance(_elementsSettingsDB).AsSingle();

            Container.Bind<MainMenuPanel>().FromInstance(_mainMenuPanel).AsSingle();
            Container.Bind<GameplayPanel>().FromInstance(_gameplayPanel).AsSingle();
            Container.Bind<PanelsSwitcher>().FromInstance(_panelsSwitcher).AsSingle();
            Container.Bind<GameFieldParent>().FromInstance(_gameFieldParent).AsSingle();
            Container.Bind<FieldView>().FromInstance(_gameFieldView).AsSingle();

            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<PlayerController>().AsSingle().NonLazy();
            Container.Bind<PlayerView>().FromInstance(_playerView).AsSingle();

            Container.Bind<FieldModel>().AsSingle();
            Container.Bind<FieldController>().AsSingle().NonLazy();

            Container.Bind<ElementAnimator>().AsSingle();
        }
    }
    
    public class LastAddedBalanceDate
    {
        public DateTime LastReceivedData;
        public bool IsFirstReceivedDay;
            
        public LastAddedBalanceDate()
        {
            int day = PlayerPrefs.GetInt("Day");
            int month = PlayerPrefs.GetInt("Month");
            int year = PlayerPrefs.GetInt("Year", 0);

            if (year == 0)
            {
                LastReceivedData = DateTime.Today;
                IsFirstReceivedDay = true;
                
                SaveReceivedData();
            }
            else
                LastReceivedData = new DateTime(year, month, day);
        }

        private void SaveReceivedData()
        {
            LastReceivedData = DateTime.Today;
                
            PlayerPrefs.SetInt("Day", LastReceivedData.Day);
            PlayerPrefs.SetInt("Month", LastReceivedData.Month);
            PlayerPrefs.SetInt("Year", LastReceivedData.Year); 
        }
    }
}