using KARC.WitchEngine;
using System;

namespace KARC.MVP
{
    public class GameplayPresenter
    {
        private IGameplayView _gameplayView = null;
        private IGameplayModel _gameplayModel = null;

        public GameplayPresenter(IGameplayView gameplayView, IGameplayModel gameplayModel)
        {
            _gameplayView = gameplayView;
            _gameplayModel = gameplayModel;



            _gameplayView.CycleFinished += ViewModelUpdate;
            _gameplayView.PlayerSpeedChanged += ViewModelMovePlayer;
            _gameplayView.GamePaused += ViewModelPause;
            _gameplayView.GameLaunched += ViewModelInitialize;

            _gameplayModel.Updated += ModelViewUpdate;
            _gameplayModel.GameFinished += ModelViewFinish;
            
        }

        private void ModelViewFinish(object sender, GameOverEventArgs e)
        {
            _gameplayView.ShowGameOver(e.IsWin);
        }

        private void ViewModelInitialize(object sender, InitializeEventArgs e)
        {
            _gameplayModel.Initialize(e.Resolution);
        }

        private void ViewModelPause(object sender, EventArgs e)
        {
            _gameplayModel.SwitchPause();
        }

        public void LaunchGame()
        {
            _gameplayView.Run();
        }

        private void ViewModelMovePlayer(object sender, ControlsEventArgs e)
        {
            _gameplayModel.ChangePlayerSpeed(e.Direction);
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            _gameplayView.LoadGameCycleParameters(e.Objects, e.POVShift, e.Score, e.Speed, e.DistanceToFinish);
        }


        private void ViewModelUpdate(object sender, CycleViewEventArgs e)
        {
            _gameplayModel.GameTime = e.GameTime;
            _gameplayModel.Update();
        }
    }
}

