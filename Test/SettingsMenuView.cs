using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitchEngine;
using WitchEngine.MonogamePart;
using WitchEngine.MVP;
using WitchEngine.UI;

namespace Test;

public class SettingsMenuView : View
{
    public override void Initialize()
    {
        MessageBox MbxTest = new MessageBox(new Vector2(
                Globals.Resolution.Width / 2, Globals.Resolution.Height / 2 - 100),
                LoadableObjects.GetFont("MainFont"),
                "Новое окно новой сцены!!!"
                );
        MbxTest.IsCentered = true;
        _interfaceManager.AddElement(MbxTest);
    }
    public override void Update()
    {        
        
        if (InputsManager.IsSinglePressed(Keys.Space))
            OnSceneFinished(new SceneFinishedEventArgs() { NewSceneName = "Test1" });

        
        base.Update();
    }
    public override void LoadModelData(ModelViewData currentModelData)
    {
        throw new NotImplementedException();
    }
}
