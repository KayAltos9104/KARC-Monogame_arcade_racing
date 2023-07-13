using KARC.Models;
using KARC.WitchEngine;
using KARC.WitchEngine.Primitives;
using KARC.WitchEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace KARC.MVP;
public class MenuView : IView
{
    public event EventHandler<CycleViewEventArgs> CycleFinished;

    private InterfaceController _interfaceController = new InterfaceController();
    private Dictionary<int, string> _activeElements = new Dictionary<int, string>();
    private int _cursor = 0;
    private List<Keys> _pressedPrevFrame = new List<Keys>();

    public (int Width, int Height) Resolution;
    public void Initialize()
    {
        Resolution = (Graphics2D.Graphics.PreferredBackBufferWidth,
            Graphics2D.Graphics.PreferredBackBufferHeight);

        MessageBox MbxTest = new MessageBox(new Vector2(
                Resolution.Width/2, Resolution.Height/2 - 100),
                "Тестовое окно\nНажмите Q, чтобы поменять текст"
                );
        MbxTest.IsCentered = true;

        Button BtnTest = new Button(new Vector2(
                Resolution.Width / 2, Resolution.Height / 2 + 100), "Нажми меня!");
        Button BtnTest2 = new Button(new Vector2(
                Resolution.Width / 2, Resolution.Height / 2 + 150), "Нет, меня!");
        Button BtnTest3 = new Button(new Vector2(
                Resolution.Width / 2, Resolution.Height / 2 + 250), "Выход");

        BtnTest.Click += BtnTest1_Click;
        BtnTest2.Click += BtnTest2_Click;
        BtnTest3.Click += BtnTest3_Click;


        _interfaceController.AddComponent("MbxTest", MbxTest);
        _interfaceController.AddComponent("BtnTest", BtnTest);
        _interfaceController.AddComponent("BtnTest2", BtnTest2);
        _interfaceController.AddComponent("BtnTest3", BtnTest3);
        _activeElements.Add(0, "BtnTest");
        _activeElements.Add(1, "BtnTest2");
        _activeElements.Add(2, "BtnTest3");
    }


    public void Update(GameTime gameTime)
    {
        var keys = Keyboard.GetState().GetPressedKeys();
        if (keys.Length > 0)
        {
            var k = keys[0];
            switch (k)
            {                
                case Keys.Q:
                    {
                        _interfaceController.Components["MbxTest"].Text = "Тестовое окно\nПроверка пройдена";
                        break;
                    }
            }

            
        }
        if (IsSinglePressed(Keys.Down))
        {
            ChangeCursor(Keys.Down);
        }
        if (IsSinglePressed(Keys.Up))
        {
            ChangeCursor(Keys.Up);
        }

        if (IsSinglePressed(Keys.Space))
        {
            (_interfaceController.Components[_activeElements[_cursor]] as Button).PerformClick();
        }
        foreach (var activeElement in _activeElements.Values)
        {
            (_interfaceController.Components[activeElement] as Button).IsChosen = false;
        }
        (_interfaceController.Components[_activeElements[_cursor]] as Button).IsChosen = true;
        _pressedPrevFrame = new List<Keys>(keys);
        CycleFinished?.Invoke(this, new CycleViewEventArgs() { GameTime = gameTime });
    }
    public void ChangeCursor(Keys k)
    {
        if (k == Keys.Up)
        {
            _cursor--;
            if (_cursor < 0)
            {
                _cursor = _activeElements.Count - 1;
            }         
        }
        else if (k == Keys.Down)
        {
            _cursor++;
            if (_cursor >= _activeElements.Count)
            {
                _cursor = 0;
            }
        }
    }
    public void Draw(GameTime gameTime)
    {
        Graphics2D.SpriteBatch.Begin();
        _interfaceController.RenderAll(Graphics2D.SpriteBatch);
        Graphics2D.SpriteBatch.End();
    }

    private bool IsSinglePressed(Keys key)
    {
        return Keyboard.GetState().IsKeyUp(key) && _pressedPrevFrame.Contains(key);
    }

    private void BtnTest1_Click(object sender, ClickEventArgs e)
    {
        MessageBox MbxTest1 = new MessageBox(new Vector2(
                250, 30),
                "Нажата кнопка 1"
                );
        MbxTest1.IsCentered = true;

        _interfaceController.AddComponent("MbxTest1", MbxTest1);
    }
    private void BtnTest2_Click(object sender, ClickEventArgs e)
    {
        MessageBox MbxTest2 = new MessageBox(new Vector2(
                250, 60),
                "Нажата кнопка 2"
                );
        MbxTest2.IsCentered = true;

        _interfaceController.AddComponent("MbxTest2", MbxTest2);
    }
    private void BtnTest3_Click(object sender, ClickEventArgs e)
    {
        MessageBox MbxTest3 = new MessageBox(new Vector2(
                250, 90),
                "Заглушка на выход из программы"
                );
        MbxTest3.IsCentered = true;

        _interfaceController.AddComponent("MbxTest3", MbxTest3);
    }
}
