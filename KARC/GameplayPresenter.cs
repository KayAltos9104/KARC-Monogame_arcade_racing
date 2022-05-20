using System;

namespace KARC
{
    public class GameplayPresenter
    {
        private IGameplayView _gameplayView = null;
        private IGameplayModel _gameplayModel = null;

        public GameplayPresenter (IGameplayView gameplayView, IGameplayModel gameplayModel)
        {
            _gameplayView = gameplayView;
            _gameplayModel = gameplayModel;

           

            _gameplayView.CycleFinished += ViewModelUpdate;
            _gameplayView.PlayerSpeedChanged += ViewModelMovePlayer;
            _gameplayModel.Updated += ModelViewUpdate;            

            _gameplayModel.Initialize();
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
            _gameplayView.LoadGameCycleParameters(e.Objects, e.POVShift);
        }


        private void ViewModelUpdate(object sender, EventArgs e)
        {
            _gameplayModel.Update();
        }
    }
}
