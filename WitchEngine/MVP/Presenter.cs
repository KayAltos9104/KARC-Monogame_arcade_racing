using WitchEngine.MonogamePart;
using static WitchEngine.MVP.Model;

namespace WitchEngine.MVP;

public sealed class Presenter
{
    private GameProcessor _processor;
    private View _view;
    private Model _model;

    public Presenter (GameProcessor processor, View view, Model model)
    {
        _processor = processor;
        _view = view;
        _model = model;
        
        _view.CycleFinished += UpdateModel;
        _view.SceneFinished += SwitchScene;

        if (_model != null) 
        {
            _model.CycleFinished += LoadModelDataToView;
        }
        
    }

    private void UpdateModel(object sender, ViewCycleFinishedEventArgs e)
    {
        if (_model != null) _model.Update(e);
    }

    private void LoadModelDataToView (object sender, ModelCycleFinishedEventArgs e)
    {
        if (_model != null) _view.LoadModelData(e.ModelViewData);
    }

    private void SwitchScene (object sender, SceneFinishedEventArgs e)
    {
        _processor.SetCurrentScene(e.NewSceneName);
    }
}
