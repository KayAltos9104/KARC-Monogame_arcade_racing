using KARC.Models;
using KARC.WitchEngine;
using KARC.WitchEngine.Primitives;
using KARC.WitchEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace KARC.MVP;
public class MenuView : IView
{
    public event EventHandler<CycleViewEventArgs> CycleFinished;

    private InterfaceController _interfaceController = new InterfaceController();
    private int _cursor = 0;

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
        _interfaceController.AddComponent("MbxTest", MbxTest);
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
    }
    public void Draw(GameTime gameTime)
    {
        Graphics2D.SpriteBatch.Begin();
        _interfaceController.RenderAll(Graphics2D.SpriteBatch);
        Graphics2D.SpriteBatch.End();
    }   
}
