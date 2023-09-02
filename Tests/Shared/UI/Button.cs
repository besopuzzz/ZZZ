using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using ZZZ.Framework;
using ZZZ.Framework.Main.UI;

public class Button : Label
{
    public event EventHandler<Button, MouseState> OnButtonClick;
    public Color FocusColor { get; set; } = Color.BlueViolet;

    private MouseState oldState;

    protected override void MouseEnter(MouseState mouseState)
    {
        if (mouseState.LeftButton == ButtonState.Pressed & oldState.LeftButton == ButtonState.Released)
        {
            ButtonClick();
            OnButtonClick?.Invoke(this, mouseState);
        }

        oldState = mouseState;

        base.MouseEnter(mouseState);
    }

    protected virtual void ButtonClick()
    {

    }
}