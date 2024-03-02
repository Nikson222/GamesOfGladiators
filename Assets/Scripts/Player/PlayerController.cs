using Enums;
using MainMenu;

namespace Player
{
    public class PlayerController
    {
        private PlayerView _playerView;
        private PlayerModel _playerModel;

        public PlayerController(PlayerView playerView, PlayerModel playerModel)
        {
            _playerView = playerView;
            _playerModel = playerModel;
            
            _playerModel.OnBalanceUpdated += _playerView.UpdateBalance;
            _playerModel.OnBetUpdated += _playerView.UpdateBet;
            _playerModel.OnAutoPlayCountUpdated += _playerView.UpdateAutoPlayCount;
            
            _playerView.MinusBetButton.onClick.AddListener(_playerModel.RemoveBet);
            _playerView.PlusBetButton.onClick.AddListener(_playerModel.AddBet);
            
            _playerView.MinusAutoPlayButton.onClick.AddListener(_playerModel.RemoveAutoPlayCount);
            _playerView.PlusAutoPlayButton.onClick.AddListener(_playerModel.AddAutoPlayCount);
            
            _playerModel.SendUpdateSignal();
        }
    }
}