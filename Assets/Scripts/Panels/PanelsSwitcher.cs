using Signals;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class PanelsSwitcher : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _gamePanel;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnGamePanelShowSignal>(ShowGame);
            _signalBus.Subscribe<OnMainMenuShowSignal>(ShowMainMenu);
        }
        
        public void ShowMainMenu()
        {
            _mainMenuPanel.SetActive(true);
            _gamePanel.SetActive(false);
        }

        public void ShowGame()
        {
            _mainMenuPanel.SetActive(false);
            _gamePanel.SetActive(true);
            
            _signalBus.Fire<OnGamePanelShowedSignal>();
        }
    }
}