using WitchEngine.MonogamePart;
using static WitchEngine.MVP.Model;

namespace WitchEngine.MVP;
/// <summary>
/// Presenter class which connects <see cref="View"/> and <see cref="Model"/> in <see cref="Scene"/>
/// </summary>
public sealed class Presenter
{
    private readonly GameProcessor _processor;
    private readonly View _view;
    private readonly Model? _model;

    public Presenter(GameProcessor processor, View view, Model model)
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
    /// <summary>
    /// Updates <see cref="_model"/> game logic after <see cref="View"/> finished its cycle
    /// </summary>
    /// <param name="sender">Sender of the <c>_view.CycleFinished</c> event</param>
    /// <param name="e">Data from <see cref="_view"/></param>
    private void UpdateModel(object? sender, ViewCycleFinishedEventArgs e)
    {
        if (_model != null) 
            _model.Update(e);
    }
    /// <summary>
    /// Loads data from <see cref="_model"/> to <see cref="_view"/> after model finishes its cycle
    /// </summary>
    /// <param name="sender">Sender of the <c>_model.CycleFinished</c> event</param>
    /// <param name="e">Data from <see cref="_model"/></param>
    private void LoadModelDataToView (object? sender, ModelCycleFinishedEventArgs e)
    {
        if (_model != null) 
            _view.LoadModelData(e.ModelViewData);
    }
    /// <summary>
    /// Changes current <see cref="Scene"/>
    /// </summary>
    /// <param name="sender">Sender of the <c>_view.SceneFinished</c> event</param>
    /// <param name="e">Data from <see cref="_view"/></param>
    /// <remarks>
    /// Accesses to <see cref="GameProcessor"/>
    /// </remarks>
    private void SwitchScene (object? sender, SceneFinishedEventArgs e)
    {
        _processor.SetCurrentScene(e.NewSceneName);
    }
}
