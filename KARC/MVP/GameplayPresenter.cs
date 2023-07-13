using KARC.WitchEngine;
using System;

namespace KARC.MVP
{
    public class GameplayPresenter : IPresenter
    {
        private GameCycleView _gameplayView = null;
        private GameCycle _gameplayModel = null;

        public GameplayPresenter(GameCycleView gameplayView, GameCycle gameplayModel)
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

        private void ViewModelMovePlayer(object sender, ControlsEventArgs e)
        {
            _gameplayModel.ChangePlayerSpeed(e.Direction);
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            var racingEventArg = (RacingEventArgs)e;
            _gameplayView.LoadGameCycleParameters(
                e.Objects, 
                e.POVShift,
                racingEventArg.Score,
                racingEventArg.Speed, 
                racingEventArg.DistanceToFinish, 
                racingEventArg.Effects);
        }


        private void ViewModelUpdate(object sender, CycleViewEventArgs e)
        {
            _gameplayModel.GameTime = e.GameTime;
            _gameplayModel.Update();
        }
    }
}

