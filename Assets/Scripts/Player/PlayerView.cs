using System;
using DG.Tweening;
using Signals;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        private SignalBus _signalBus;

        [SerializeField] private Button _spinButton;
        [SerializeField] private Button _minusBetButton;
        [SerializeField] private Button _plusBetButton;

        [SerializeField] private Text _betText;

        [FormerlySerializedAs("_winText")] [SerializeField]
        private Text _balanceText;

        [FormerlySerializedAs("_munisAutoPlayButton")] [SerializeField]
        private Button minusAutoPlayButton;

        [SerializeField] private Button _plusAutoPlayButton;
        [SerializeField] private Text _autoPlayText;

        public Button SpinButton => _spinButton;
        public Button MinusBetButton => _minusBetButton;
        public Button PlusBetButton => _plusBetButton;

        public Button MinusAutoPlayButton => minusAutoPlayButton;
        public Button PlusAutoPlayButton => _plusAutoPlayButton;
        public Text AutoPlayText => _autoPlayText;

        public Text BetText => _betText;
        public Text BalanceText => _balanceText;

        public Sequence SpinButtonSequence { get; private set; }

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<OnSuccessSpinSignal>(OnSuccessSpin);

            _signalBus.Subscribe<OnAutoRollEndedSignal>(OnRollingDullEnded);

            _spinButton.onClick.AddListener(() => _signalBus.Fire<OnSpinButtonClickSignal>());
        }

        public void UpdateBalance(int obj)
        {
            _balanceText.text = obj.ToString();
        }

        public void UpdateBet(int obj)
        {
            _betText.text = obj.ToString();
        }

        private void OnSuccessSpin()
        {
            // Воспроизводим анимацию вращения
            _spinButton.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);

            Debug.Log("OnSuccessSpin");

            // Делаем кнопку неактивной
            _spinButton.interactable = false;
        }

        private void OnRollingDullEnded()
        {
            _spinButton.transform.DOKill();
            _spinButton.transform.DORotate(Vector3.zero, 0.2f);
            Debug.Log("OnRollingDullEnded");

            _spinButton.interactable = true;
        }

        public void UpdateAutoPlayCount(int obj)
        {
            _autoPlayText.text = obj.ToString();
        }
    }
}