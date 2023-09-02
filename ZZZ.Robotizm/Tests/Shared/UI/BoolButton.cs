using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class BoolButton : Button
{
    public bool State => state;
    public Color StateColor { get; set; }= Color.Blue;

    private bool state;


    protected override void ButtonClick()
    {

        state = !state;

        if (state)
            BackgroundColor = StateColor;

        base.ButtonClick();
    }

    protected override void MouseEnter(MouseState mouseState)
    {       

        if (state)
            BackgroundColor = StateColor;
        else BackgroundColor = FocusColor;

        base.MouseEnter(mouseState);

    }

    protected override void MouseExit(MouseState mouseState)
    {
        if (!state)
            BackgroundColor = Color.White;
    }
}