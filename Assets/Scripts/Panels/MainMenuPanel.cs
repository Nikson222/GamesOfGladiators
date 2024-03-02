using System;
using Enums;
using Field;
using Player;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    public class MainMenuPanel : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [SerializeField] private Button _chooseField4x3Button;
        [SerializeField] private Button _chooseField5x4Button;
        [SerializeField] private Button _chooseField6x5Button;
        [SerializeField] private Button _chooseField7x6Button;
        [SerializeField] private Button _startButton;

        [Inject]
        private void Construct(SignalBus signalBus, FieldModel fieldModel)
        {
            _signalBus = signalBus;

            switch (fieldModel.FieldSize)
            {
                case FieldSizeEnum.Raws3Columns4:
                    _chooseField4x3Button.onClick.Invoke();
                    break;
                case FieldSizeEnum.Raws4Columns5:
                    _chooseField5x4Button.onClick.Invoke();
                    break;
                case FieldSizeEnum.Raws5Columns6:
                    _chooseField6x5Button.onClick.Invoke();
                    break;
                case FieldSizeEnum.Raws6Columns7:
                    _chooseField7x6Button.onClick.Invoke();
                    break;
            }
        }
        
        private void Start()
        {
            _startButton.onClick.AddListener(() => _signalBus.Fire<OnGamePanelShowSignal>());
        }
        
        public void AddListenerField4x3Button(Action action) =>
            _chooseField4x3Button.onClick.AddListener(() => action());

        public void AddListenerField5x4Button(Action action) =>
            _chooseField5x4Button.onClick.AddListener(() => action());

        public void AddListenerField6x5Button(Action action) =>
            _chooseField6x5Button.onClick.AddListener(() => action());

        public void AddListenerField7x6Button(Action action) =>
            _chooseField7x6Button.onClick.AddListener(() => action());
        
        public void AddListenerStartButton(Action action) =>
            _startButton.onClick.AddListener(() => action());
    }
}