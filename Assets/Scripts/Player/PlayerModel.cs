using System;
using Enums;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerModel
    {
        private int _currentBalance;
        private int _currentBet;
        private int _currentAutoPlayCount = 1;
        
        private const int BET_STEP = 500;
        
        private readonly SignalBus _signalBus;

        public event Action<int> OnBalanceUpdated;
        public event Action<int> OnBetUpdated;
        public event Action<int> OnAutoPlayCountUpdated;

        public PlayerModel(SignalBus signalBus)
        {
            _currentBalance = PlayerPrefs.GetInt("Balance", 10000);
            _currentBet = BET_STEP;
            
            _signalBus = signalBus;
            _signalBus.Subscribe<OnSpinButtonClickSignal>(RemoveBalance);
            _signalBus.Subscribe<OnWinningRelustRollingSignal>(AddBalance);
            
            _signalBus.Subscribe<OnRollingFullEndedSignal>(RollAutoPlay);
        }

        public void SendUpdateSignal()
        {
            OnBalanceUpdated?.Invoke(_currentBalance);
            OnBetUpdated?.Invoke(_currentBet);
            OnAutoPlayCountUpdated?.Invoke(_currentAutoPlayCount);
        }

        private void AddBalance(OnWinningRelustRollingSignal signal)
        {
            if (_currentBet * signal.coefficient > 0)
            {
                _currentBalance += (int)(_currentBet * signal.coefficient);
                PlayerPrefs.SetInt("Balance", _currentBalance);
                OnBalanceUpdated?.Invoke(_currentBalance);
            }
        }

        private void RemoveBalance()
        {
            if (_currentBalance >= _currentBet)
            {
                _currentBalance -= _currentBet;
                _signalBus.Fire<OnSuccessSpinSignal>(new OnSuccessSpinSignal(_currentAutoPlayCount));
                PlayerPrefs.SetInt("Balance", _currentBalance);
                OnBalanceUpdated?.Invoke(_currentBalance);
            }
        }

        public void AddBet()
        {
            _currentBet += BET_STEP;
            OnBetUpdated?.Invoke(_currentBet);
        }

        public void RemoveBet()
        {
            if (_currentBet > BET_STEP)
            {
                _currentBet -= BET_STEP;
                OnBetUpdated?.Invoke(_currentBet);
            }
        }

        public void AddAutoPlayCount()
        {
            _currentAutoPlayCount++;
            OnAutoPlayCountUpdated?.Invoke(_currentAutoPlayCount);
        }

        public void RemoveAutoPlayCount()
        {
            if (_currentAutoPlayCount > 1)
            {
                _currentAutoPlayCount--;
                OnAutoPlayCountUpdated?.Invoke(_currentAutoPlayCount);
            }
        }


        private void RollAutoPlay()
        {
            if (_currentAutoPlayCount > 1)
            {
                RemoveAutoPlayCount();
                _signalBus.Fire<OnSpinButtonClickSignal>();
            }
            else
            {
                _signalBus.Fire<OnAutoRollEndedSignal>();
            }
        }
    }
}