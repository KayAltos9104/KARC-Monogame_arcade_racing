using static WitchEngine.MVP.Model;

namespace WitchEngine.MVP;

public sealed class Presenter
{
    private View View { get; set; }
    private Model Model { get; set; }

    public Presenter (View view, Model model)
    {
        View = view;
        Model = model;
        View.CycleFinished += UpdateModel;
        Model.CycleFinished += LoadModelDataToView;
    }

    private void UpdateModel(object sender, ViewCycleFinishedEventArgs e)
    {
        Model.Update(e);
    }

    private void LoadModelDataToView (object sender, ModelCycleFinishedEventArgs e)
    {
        View.LoadModelData(e.ModelViewData);
    }
}
