using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using WitchEngine;
using WitchEngine.MonogamePart;
using WitchEngine.MVP;
using WitchEngine.UI;

namespace Test;

public class MainMenuView : View, IKeyboardCursor
{
    public List<IComponent> InteractiveElements { get; set; } = new List<IComponent>();

    public int CursorPos { get; set; } = 0;

    public override void Initialize()
    {
        MessageBox MbxTest = new MessageBox(new Vector2(
                Globals.Resolution.Width / 2, Globals.Resolution.Height / 2 - 100),
                LoadableObjects.GetFont("MainFont"),
                "Тестовое окно\nНажмите Q, чтобы поменять текст"
                );
        MbxTest.IsCentered = true;

        Button BtnTest = new Button(new Vector2(
                Globals.Resolution.Width / 2, Globals.Resolution.Height / 2 + 100), 
                LoadableObjects.GetFont("MainFont"), "Перейти к сцене 2");
        Button BtnTest2 = new Button(new Vector2(
                Globals.Resolution.Width / 2, Globals.Resolution.Height / 2 + 150), 
                LoadableObjects.GetFont("MainFont"), "Нет, меня!");
        Button BtnTest3 = new Button(new Vector2(
                Globals.Resolution.Width / 2, Globals.Resolution.Height / 2 + 250), 
                LoadableObjects.GetFont("MainFont"), "Выход");

        BtnTest.Click += BtnTest1_Click;
        BtnTest2.Click += BtnTest2_Click;
        BtnTest3.Click += BtnTest3_Click;

        _interfaceElements.Add("MbxTest", MbxTest);
        _interfaceElements.Add("BtnTest", BtnTest);
        InteractiveElements.Add(BtnTest);  
        _interfaceElements.Add("BtnTest2", BtnTest2);
        InteractiveElements.Add(BtnTest2);
        _interfaceElements.Add("BtnTest3", BtnTest3);
        InteractiveElements.Add(BtnTest3);
        ((IKeyboardCursor)this).UpdateActivationOnElement();
    }

    public override void Update()
    {
        ReadInputs();
       
        if (IsSinglePressed(Keys.W))
            ((IKeyboardCursor)this).MoveCursor(DiscreteDirection.Up);
        if (IsSinglePressed(Keys.S))
            ((IKeyboardCursor)this).MoveCursor(DiscreteDirection.Down);
        if (IsSinglePressed(Keys.Space))
            (((IKeyboardCursor)this).GetCurrentElement() as Button).PerformClick();

        SaveInputs();
        base.Update();
    }

    public override void LoadModelData(ModelViewData currentModelData)
    {
        throw new NotImplementedException();
    }
    private void BtnTest1_Click(object sender, ClickEventArgs e)
    {
        //MessageBox MbxTest1 = new MessageBox(new Vector2(
        //        250, 30),
        //        LoadableObjects.GetFont("MainFont"),
        //        "Нажата кнопка 1"
        //        );
        //MbxTest1.IsCentered = true;

        //_interfaceElements.Add("MbxTest1", MbxTest1);
        OnSceneFinished(new SceneFinishedEventArgs() { NewSceneName = "Test2" });
    }
    private void BtnTest2_Click(object sender, ClickEventArgs e)
    {
        MessageBox MbxTest2 = new MessageBox(new Vector2(
                250, 60),
                LoadableObjects.GetFont("MainFont"),
                "Нажата кнопка 2"
                );
        MbxTest2.IsCentered = true;

        _interfaceElements.Add("MbxTest2", MbxTest2);
    }
    private void BtnTest3_Click(object sender, ClickEventArgs e)
    {
        MessageBox MbxTest3 = new MessageBox(new Vector2(
                250, 90),
                LoadableObjects.GetFont("MainFont"),
                "Заглушка на выход из программы"
                );
        MbxTest3.IsCentered = true;

        _interfaceElements.Add("MbxTest3", MbxTest3);
    }
}
