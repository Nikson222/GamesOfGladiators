using System.Collections;
using System.Collections.Generic;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayPanel : MonoBehaviour
{
    private SignalBus _signalBus;
    
    [SerializeField] private Button _backButton;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
        
        _backButton.onClick.AddListener(() => _signalBus.Fire<OnMainMenuShowSignal>());
        
        _signalBus.Subscribe<OnSuccessSpinSignal>(() => _backButton.interactable = false);

        _signalBus.Subscribe<OnAutoRollEndedSignal>(() => _backButton.interactable = true);
    }
}
